import { gql } from "@apollo/client";

// we return the featured property as well so that when the featured is updated by this mutation the cache will be updated as with new value of featured
export const FEATURED_SPEAKER = gql`
  mutation markFeatured($speakerId: ID!, $featured: Boolean!) {
    markFeatured(speakerId: $speakerId, featured: $featured) {
      id
      featured
    }
  }
`;
