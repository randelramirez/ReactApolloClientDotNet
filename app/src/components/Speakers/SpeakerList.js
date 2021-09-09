import React from "react";
import { useQuery, useMutation } from "@apollo/client";
import { FEATURED_SPEAKER, SPEAKERS } from "./queries";

export default function SpeakerList() {
  const { loading, error, data } = useQuery(SPEAKERS);

  const [markFeatured] = useMutation(FEATURED_SPEAKER);

  if (loading) return <p>Loading speakers...</p>;
  if (error) return <p>Error loading speakers!</p>;

  return data.speakers.map(({ id, name, bio, sessions, featured }) => (
    <div
      key={id}
      className="col-xs-12 col-sm-6 col-md-6"
      style={{ padding: 5 }}
    >
      <div className="panel panel-default">
        <div className="panel-heading">
          <h3 className="panel-title">{"Speaker: " + name}</h3>
        </div>
        <div className="panel-body">
          <h5>{"Bio: " + bio}</h5>
        </div>
        <div className="panel-footer">
          <h4>Sessions</h4>
          {sessions.map((session) => (
            <span key={session.id}>
              <p>{session.title}</p>
            </span>
          ))}
          <span>
            <button
              type="button"
              className="btn btn-default btn-lg"
              onClick={async () => {
                await markFeatured({
                  variables: {
                    speakerId: id,
                    featured: true,
                  },
                });
              }}
            >
              <i
                className={`fa ${featured ? "fa-star" : "fa-star-o"}`}
                aria-hidden="true"
                style={{
                  color: featured ? "gold" : undefined,
                }}
              ></i>{" "}
              Featured Speaker
            </button>
          </span>
        </div>
      </div>
    </div>
  ));
}
