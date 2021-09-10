import React from "react";
import { useQuery } from "@apollo/client";
import { ALL_SESSIONS } from "./queries";
import SessionItem from "./SessionItem";

export default function AllSessionList() {
  const { loading, error, data } = useQuery(ALL_SESSIONS);

  if (loading) return <p>Loading Sessions..</p>;

  if (error) return <p>Error loading sessions!</p>;

  return data.sessions.map((session) => (
    <SessionItem
      key={session.id}
      session={{
        ...session,
      }}
    />
  ));
}
