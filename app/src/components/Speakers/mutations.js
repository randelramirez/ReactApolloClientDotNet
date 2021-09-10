import { gql } from "@apollo/client";

export const FEATURED_SPEAKER = gql`
  mutation markFeatured($speakerId: ID!, $featured: Boolean!) {
    markFeatured(speakerId: $speakerId, featured: $featured) {
      id
      featured
    }
  }
`;
