using Core.Data;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.BookDetailQueries
{
    public class BookDetail_GetBookByIdQuery
    {
        public class Query : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Language { get; set; }
            public string Level { get; set; }
            public string Content { get; set; }
            public Guid AuthorId { get; set; }
            public List<byte[]> Images { get; set; } = new();

            // New fields for author's name and surname:
            public string AuthorName { get; set; }
            public string AuthorSurname { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Books
                    .Include(b => b.Images)
                    .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                if (book == null)
                    return null;

                // Fetch the author's name & surname:
                var author = await _context.Users
                    .Where(u => u.Id == book.AuthorId)
                    .Select(u => new { u.Name, u.Surname })
                    .FirstOrDefaultAsync(cancellationToken);

                return new Response
                {
                    Id = book.Id,
                    Title = book.Title,
                    Language = book.Language,
                    Level = book.Level,
                    Content = book.Content,
                    AuthorId = book.AuthorId,
                    Images = book.Images.Select(i => i.ImageData).ToList(),
                    AuthorName = author?.Name ?? "",
                    AuthorSurname = author?.Surname ?? ""
                };
            }
        }
    }

    public class BookDetail_GetCommentsByBookIdQuery
    {
        public class Query : IRequest<List<Response>>
        {
            public Guid BookId { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public string Text { get; set; }
            public List<ReactionDto> Reactions { get; set; } = new();

            // New fields for the commenter's name & surname:
            public string UserName { get; set; }
            public string UserSurname { get; set; }
        }

        public class ReactionDto
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public ReactionType ReactionType { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Response>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                // First, load the comment data (excluding user info):
                var comments = await _context.Comments
                    .Include(c => c.Reactions)
                    .Where(c => c.BookId == request.BookId)
                    .Select(c => new Response
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        Text = c.Text,
                        Reactions = c.Reactions
                            .Select(r => new ReactionDto
                            {
                                Id = r.Id,
                                UserId = r.UserId,
                                ReactionType = r.ReactionType
                            })
                            .ToList()
                    })
                    .ToListAsync(cancellationToken);

                if (!comments.Any())
                    return comments; // No comments, just return empty

                // Collect all user IDs who commented:
                var userIds = comments.Select(c => c.UserId).Distinct().ToList();

                // Load the user name/surname from the Users table:
                var userDict = await _context.Users
                    .Where(u => userIds.Contains(u.Id))
                    .Select(u => new { u.Id, u.Name, u.Surname })
                    .ToDictionaryAsync(u => u.Id, cancellationToken);

                // Match each comment to its user's name/surname:
                foreach (var comment in comments)
                {
                    if (userDict.TryGetValue(comment.UserId, out var user))
                    {
                        comment.UserName = user.Name;
                        comment.UserSurname = user.Surname;
                    }
                }

                return comments;
            }
        }
    }

    public class BookDetail_GetReactionsByBookIdQuery
    {
        public class Query : IRequest<List<Response>>
        {
            public Guid BookId { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public ReactionType ReactionType { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Response>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var reactions = await _context.Reactions
                //    .Where(r => r.BookId == request.BookId)
                //    .Select(r => new Response
                //    {
                //        Id = r.Id,
                //        UserId = r.UserId,
                //        ReactionType = r.ReactionType
                //    })
                //    .ToListAsync(cancellationToken);
                //burada commentıd si null olmalı geri dönerken
                var reactions = await _context.Reactions
                    .Where(r => r.BookId == request.BookId && r.CommentId == null)
                    .Select(r => new Response
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        ReactionType = r.ReactionType
                    })
                    .ToListAsync(cancellationToken);

                return reactions;
            }
        }
    }

    public class Profile_GetUsersBooks
    {
        public class Query : IRequest<List<Response>>
        {
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Language { get; set; }
            public string Level { get; set; }
            public string? ImageUrl { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Response>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Books
                    .Where(b => b.AuthorId == request.UserId)
                    .Select(b => new Response
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Language = b.Language,
                        Level = b.Level,
                        //get first image
                        ImageUrl = b.Images.FirstOrDefault() != null ? Convert.ToBase64String(b.Images.FirstOrDefault().ImageData) : null

                    })
                    .ToListAsync(cancellationToken);

                return books;
            }
        }
    }
}
