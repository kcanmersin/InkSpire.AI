import { gql } from "@apollo/client";

export const MAIN_PAGE_BOOK_LIST = gql`
  query mainPageBookList(
    $limit: Int
    $offset: Int
    $languages: [String!]
    $levels: [String!]
  ) {
    books(
      limit: $limit
      offset: $offset
      languages: $languages
      levels: $levels
    ) {
      id
      title
      language
      level
      images {
        imageData
      }
    }
  }
`;

export const GET_BOOK_BY_ID = gql`
  query getBookById($id: UUID!) {
    bookById(id: $id) {
      id
      title
      language
      level
      content
      author {
        id
        name
        surname
      }
      images {
        imageData
      }
    }
  }
`;

export const GET_COMMENTS_BY_BOOK_ID = gql`
  query getCommentsByBookId($bookId: UUID!) {
    commentsByBookId(bookId: $bookId) {
      id
      text
      user {
        id
        name
        surname
      }
      reactions {
        userId
        reactionType
      }
    }
  }
`;

export const GET_REACTIONS_BY_BOOK_ID = gql`
  query getReactionsByBookId($bookId: UUID!) {
    reactionsByBookId(bookId: $bookId) {
      userId
      reactionType
    }
  }
`;
