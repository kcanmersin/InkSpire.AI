import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { useAuth } from "../context/AuthContext";
import TestModal from "../components/TestModal";
import TranslationPopup from "../components/TranslationPopup";

export enum ReactionType {
  Like = 0,
  Dislike = 1,
}

interface BookDetailData {
  id: string;
  title: string;
  language: string;
  level: string;
  content: string;
  images: string[];
  authorId?: string;
  authorName?: string;
  authorSurname?: string;
}

interface CommentReaction {
  id?: string;
  userId: string;
  reactionType: ReactionType;
}

interface Comment {
  id: string;
  userId: string;
  text: string;
  userName?: string;
  userSurname?: string;
  profileImageUrl?: string;
  reactions: CommentReaction[];
}

interface Reaction {
  id?: string;
  userId: string;
  reactionType: ReactionType;
  bookId?: string;
  commentId?: string | null;
}

const BookDetail: React.FC = () => {
  const [showModal, setShowModal] = useState(false);
  const { userId } = useAuth();
  const { id } = useParams();
  const [book, setBook] = useState<BookDetailData | null>(null);
  const [comments, setComments] = useState<Comment[]>([]);
  const [reactions, setReactions] = useState<Reaction[]>([]);
  const [pages, setPages] = useState<Array<{ type: "text" | "image"; content: string }>>([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [newCommentText, setNewCommentText] = useState("");
  const [selectedText, setSelectedText] = useState<string | null>(null);

  const handleTextSelection = () => {
    const selection = window.getSelection();
    if (selection) {
      const txt = selection.toString().trim();
      if (txt.length > 0) {
        setSelectedText(txt);
      }
    }
  };

  useEffect(() => {
    if (!id) return;
    fetch(`http://localhost:5256/api/Book/details/${id}`)
      .then(async res => {
        if (!res.ok) {
          const errText = await res.text();
          throw new Error(errText);
        }
        return res.json();
      })
      .then((data: BookDetailData) => {
        setBook(data);
      })
      .catch(err => console.error(err));
    fetch(`http://localhost:5256/api/Book/comments/${id}`)
      .then(async res => {
        if (!res.ok) {
          const errText = await res.text();
          throw new Error(errText);
        }
        return res.json();
      })
      .then((data: Comment[]) => {
        setComments(data);
      })
      .catch(err => console.error(err));
    fetch(`http://localhost:5256/api/Book/reactions/${id}`)
      .then(async res => {
        if (!res.ok) {
          const errText = await res.text();
          throw new Error(errText);
        }
        return res.json();
      })
      .then((data: Reaction[]) => {
        setReactions(data);
      })
      .catch(err => console.error(err));
  }, [id]);

  useEffect(() => {
    if (!book) return;
    const paragraphs = (book.content || "")
      .split("\n")
      .map(p => p.trim())
      .filter(p => p.length > 0);
    const newPages: Array<{ type: "text" | "image"; content: string }> = [];
    let textIndex = 0;
    let imgIndex = 0;
    while (textIndex < paragraphs.length || imgIndex < book.images.length) {
      if (textIndex < paragraphs.length) {
        const combined = paragraphs.slice(textIndex, textIndex + 2).join("\n\n");
        newPages.push({ type: "text", content: combined });
        textIndex += 2;
      }
      if (imgIndex < book.images.length) {
        newPages.push({ type: "image", content: book.images[imgIndex] });
        imgIndex++;
      }
    }
    setPages(newPages);
    setCurrentPage(0);
  }, [book]);

  const handlePreviousPage = () => {
    setCurrentPage(prev => (prev - 2 >= 0 ? prev - 2 : 0));
  };

  const handleNextPage = () => {
    setCurrentPage(prev => (prev + 2 < pages.length ? prev + 2 : prev));
  };

  const renderPage = (pageIndex: number) => {
    if (pageIndex >= pages.length) return <div className="text-muted">The END</div>;
    const page = pages[pageIndex];
    if (page.type === "text") return <div style={{ whiteSpace: "pre-line" }}>{page.content}</div>;
    return <img src={`data:image/jpeg;base64,${page.content}`} alt="" style={{ width: "100%", objectFit: "contain" }} />;
  };

  const findReaction = (commentId?: string | null) => {
    if (commentId) {
      const c = comments.find(x => x.id === commentId);
      return c?.reactions.find(r => r.userId === userId);
    }
    return reactions.find(r => r.userId === userId);
  };

  async function createReaction(rt: ReactionType, bId: string, cId?: string | null) {
    const payload = { userId, reactionType: rt, bookId: bId, commentId: cId ?? null };
    try {
      const res = await fetch("http://localhost:5256/api/Reaction/create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });
      if (!res.ok) {
        const errText = await res.text();
        throw new Error(errText);
      }
      const [uc, ur] = await Promise.all([
        fetch(`http://localhost:5256/api/Book/comments/${bId}`),
        fetch(`http://localhost:5256/api/Book/reactions/${bId}`),
      ]);
      if (!uc.ok || !ur.ok) throw new Error("Refresh failed");
      const [ucData, urData] = await Promise.all([uc.json(), ur.json()]);
      setComments(ucData);
      setReactions(urData);
    } catch (err) {
      console.error(err);
    }
  }

  const handleBookLike = () => {
    if (!book) return;
    createReaction(ReactionType.Like, book.id, null);
  };

  const handleBookDislike = () => {
    if (!book) return;
    createReaction(ReactionType.Dislike, book.id, null);
  };

  const handleCommentLike = (cid: string) => {
    if (!book) return;
    createReaction(ReactionType.Like, book.id, cid);
  };

  const handleCommentDislike = (cid: string) => {
    if (!book) return;
    createReaction(ReactionType.Dislike, book.id, cid);
  };

  async function handleCommentSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!book || !newCommentText.trim()) return;
    const payload = { userId, text: newCommentText.trim(), bookId: book.id };
    try {
      const res = await fetch("http://localhost:5256/api/Comment/create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });
      if (!res.ok) {
        const errText = await res.text();
        throw new Error(errText);
      }
      const updatedCommentsRes = await fetch(`http://localhost:5256/api/Book/comments/${book.id}`);
      if (!updatedCommentsRes.ok) {
        const errText = await updatedCommentsRes.text();
        throw new Error(errText);
      }
      const updatedComments = await updatedCommentsRes.json();
      setComments(updatedComments);
      setNewCommentText("");
    } catch (err) {
      console.error(err);
    }
  }

  if (!book) return <p>Loading book details...</p>;

  const userBookReaction = findReaction(null);
  const userBookLike = userBookReaction?.reactionType === ReactionType.Like;
  const userBookDislike = userBookReaction?.reactionType === ReactionType.Dislike;
  const bookLikeCount = reactions.filter(r => r.reactionType === ReactionType.Like).length;
  const bookDislikeCount = reactions.filter(r => r.reactionType === ReactionType.Dislike).length;

  return (
    <div className="container my-4" onMouseUp={handleTextSelection}>
      <div className="card mb-4 border-0 shadow-sm">
        <div className="card-body text-center">
          <h3 className="text-primary">{book.title}</h3>
          <hr />
          <div className="d-flex flex-wrap justify-content-center gap-3">
            <p className="mb-0"><strong>Language:</strong> {book.language}</p>
            <p className="mb-0"><strong>Level:</strong> {book.level}</p>
          </div>
          {book.authorName && book.authorSurname && (
            <p className="text-secondary mt-3">
              <strong>Author:</strong> {book.authorName} {book.authorSurname}
            </p>
          )}
          <div className="d-flex flex-wrap justify-content-between align-items-center mt-4">
            <button className="btn btn-primary fw-bold" onClick={() => setShowModal(true)}>Open Test</button>
            {showModal && book?.id && (
              <TestModal bookId={book.id} userId={userId ?? ""} onClose={() => setShowModal(false)} />
            )}
            <div className="btn-group">
              <button
                className={`btn fw-bold ${userBookLike ? "btn-success" : "btn-outline-success"}`}
                onClick={handleBookLike}
              >
                👍 Like ({bookLikeCount})
              </button>
              <button
                className={`btn fw-bold ${userBookDislike ? "btn-danger" : "btn-outline-danger"}`}
                onClick={handleBookDislike}
              >
                👎 Dislike ({bookDislikeCount})
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="text-center mb-4">
        <button className="btn btn-secondary me-2" onClick={handlePreviousPage} disabled={currentPage <= 0}>← Previous</button>
        <button className="btn btn-secondary" onClick={handleNextPage} disabled={currentPage + 2 >= pages.length}>Next →</button>
      </div>
      <div className="row justify-content-center mb-5">
        <div className="col-md-5 col-sm-12 mb-3">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center p-4" style={{ minHeight: "400px" }}>
              {renderPage(currentPage)}
            </div>
          </div>
        </div>
        <div className="col-md-5 col-sm-12 mb-3">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center p-4" style={{ minHeight: "400px" }}>
              {renderPage(currentPage + 1)}
            </div>
          </div>
        </div>
      </div>
      <h4 className="text-primary">Comments</h4>
      <div className="col-12 col-md-8 mx-auto">
        {comments.length === 0 && <p className="text-muted text-center">No comments yet.</p>}
        {comments.map(comment => {
          const userReaction = findReaction(comment.id);
          const userLike = userReaction?.reactionType === ReactionType.Like;
          const userDislike = userReaction?.reactionType === ReactionType.Dislike;
          const likeCount = comment.reactions.filter(r => r.reactionType === ReactionType.Like).length;
          const dislikeCount = comment.reactions.filter(r => r.reactionType === ReactionType.Dislike).length;
          return (
            <div key={comment.id} className="card mb-3 border-0 shadow-sm">
              <div className="card-body">
                <div className="d-flex align-items-center mb-2">
                  {comment.profileImageUrl && (
                    <img
                      src={comment.profileImageUrl}
                      alt=""
                      style={{ width: "80px", height: "80px", objectFit: "cover", borderRadius: "50%", marginRight: "10px" }}
                    />
                  )}
                  <span className="fw-bold text-primary">
                    {comment.userName && comment.userSurname
                      ? `${comment.userName} ${comment.userSurname}`
                      : `User: ${comment.userId}`}
                  </span>
                </div>
                <p className="fs-5 px-2">{comment.text}</p>
                <div className="d-flex align-items-center mt-2">
                  <button
                    className={`btn me-2 ${userLike ? "btn-success" : "btn-outline-success"}`}
                    onClick={() => handleCommentLike(comment.id)}
                  >
                    👍 Like ({likeCount})
                  </button>
                  <button
                    className={`btn me-2 ${userDislike ? "btn-danger" : "btn-outline-danger"}`}
                    onClick={() => handleCommentDislike(comment.id)}
                  >
                    👎 Dislike ({dislikeCount})
                  </button>
                </div>
              </div>
            </div>
          );
        })}
        <div className="card border-0 shadow-sm mt-4">
          <div className="card-body">
            <h5 className="text-primary">Add a Comment</h5>
            <form onSubmit={handleCommentSubmit}>
              <div className="mb-3">
                <textarea
                  id="commentText"
                  className="form-control"
                  rows={3}
                  value={newCommentText}
                  onChange={e => setNewCommentText(e.target.value)}
                  placeholder="Write your comment here..."
                />
              </div>
              <button type="submit" className="btn btn-primary">Submit Comment</button>
            </form>
          </div>
        </div>
      </div>
      {selectedText && (
        <TranslationPopup
          selectedText={selectedText}
          onClose={() => setSelectedText(null)}
          bookLanguage={book.language}
        />
      )}
    </div>
  );
};

export default BookDetail;
