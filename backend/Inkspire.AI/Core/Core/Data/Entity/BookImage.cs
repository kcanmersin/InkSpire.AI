using Core.Data.Entity.EntityBases;
using System;

namespace InkSpire.Core.Data.Entity
{
    public class BookImage : EntityBase
    {
        public Guid Id { get; private set; }
        public Guid BookId { get; private set; }
        public Book Book { get; private set; }
        public byte[] ImageData { get; private set; }

        private BookImage() { }

        public BookImage(Guid bookId, byte[] imageData)
        {
            Id = Guid.NewGuid();
            BookId = bookId;
            ImageData = imageData;
        }
    }
}
