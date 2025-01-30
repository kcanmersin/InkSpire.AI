using Core.Data.Entity.EntityBases;
using System;
using System.Collections.Generic;

namespace InkSpire.Core.Data.Entity
{
    public class Book : EntityBase
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Content { get; private set; }
        public string Language { get; private set; }
        public string Level { get; private set; }
        public Guid? TestId { get; private set; }
        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        private readonly List<Reaction> _reactions = new();
        public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();

        private readonly List<BookImage> _images = new();
        public IReadOnlyCollection<BookImage> Images => _images.AsReadOnly();

        private Book() { }

        public Book(Guid authorId, string title, string content, string language, string level)
        {
            Id = Guid.NewGuid();
            AuthorId = authorId;
            Title = title;
            Content = content;
            Language = language;
            Level = level;
        }
    }
}
