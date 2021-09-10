import { gql } from "@apollo/client";

const SPEAKER_ATTRIBUTES = gql`
  fragment SpeakerInfo on Speaker {
    id
    name
    bio
    sessions {
      id
      title
    }
    featured
  }
`;



// define speaker query
export const SPEAKERS = gql`
  query speakers {
    speakers {
      ...SpeakerInfo
    }
  }
  ${SPEAKER_ATTRIBUTES}
`;

export const SPEAKER_BY_ID = gql`
  query speakeryById($id: ID!) {
    speakerById(id: $id) {
      ...SpeakerInfo
    }
  }
  ${SPEAKER_ATTRIBUTES}
`;
