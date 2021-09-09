import React from "react";
import { useParams } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { SPEAKER_BY_ID } from "./queries";

export default function SpeakerDetails() {
  const { speaker_id } = useParams();

  const { loading, error, data } = useQuery(SPEAKER_BY_ID, {
    variables: { id: speaker_id },
  });

  if (loading) return <p>Loading speaker...</p>;
  if (error) return <p>Error loading speaker!</p>;

  const speaker = data.speakerById;
  const { id, name, bio, sessions } = speaker;

  return (
    <div key={id} className="col-xs-12" style={{ padding: 5 }}>
      <div className="panel panel-default">
        <div className="panel-heading">
          <h3 className="panel-title">{name}</h3>
        </div>
        <div className="panel-body">
          <h5>{bio}</h5>
        </div>
        <div className="panel-footer">
          {sessions.map(({ id, title }) => (
            <span key={id} style={{ padding: 5 }}>
              "{title}"
            </span>
          ))}
        </div>
      </div>
    </div>
  );
}
