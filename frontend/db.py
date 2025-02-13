import mysql.connector




def migrate_table(table_name):
    try:
        # Kaynak ve hedef veritabanlarına bağlan
        source_conn = mysql.connector.connect(**source_db_config)
        target_conn = mysql.connector.connect(**target_db_config)

        source_cursor = source_conn.cursor()
        target_cursor = target_conn.cursor()

        # Foreign key kontrollerini devre dışı bırak
        target_cursor.execute("SET FOREIGN_KEY_CHECKS = 0")

        # Hedef tabloyu temizle (isteğe bağlı)
        target_cursor.execute(f"DELETE FROM {table_name}")
        target_conn.commit()

        # Kaynak DB'deki verileri çek
        source_cursor.execute(f"SELECT * FROM {table_name}")
        rows = source_cursor.fetchall()

        if not rows:
            print(f"⚠️ Tablo {table_name} boş, veri aktarılmadı.")
            return

        # Sütun isimlerini al
        column_names = [desc[0] for desc in source_cursor.description]
        columns = ", ".join(column_names)
        values_placeholder = ", ".join(["%s"] * len(column_names))

        # Hedef tabloya verileri ekle
        insert_query = f"INSERT INTO {table_name} ({columns}) VALUES ({values_placeholder})"
        target_cursor.executemany(insert_query, rows)

        # Değişiklikleri kaydet
        target_conn.commit()
        print(f"✅ {len(rows)} satır {table_name} tablosuna aktarıldı!")

    except mysql.connector.Error as err:
        print(f"❌ Tablo {table_name} için hata: {err}")

    finally:
        # Foreign key kontrollerini tekrar etkinleştir
        target_cursor.execute("SET FOREIGN_KEY_CHECKS = 1")
        # Bağlantıları kapat
        source_cursor.close()
        target_cursor.close()
        source_conn.close()
        target_conn.close()



if __name__ == "__main__":
    print("🚀 Veri taşımaya başlandı...")
    for table in tables_to_migrate:
        migrate_table(table)
    print("✅ Veri taşıma tamamlandı!")
