import { ApolloClient, InMemoryCache} from '@apollo/client';

const client = new ApolloClient({
  // uri: 'http://localhost/graphql', 
  uri: 'http://localhost:5256/graphql', 
  cache: new InMemoryCache(),
});

export default client;
