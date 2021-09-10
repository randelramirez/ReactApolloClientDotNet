import React from "react";
import { useQuery } from "@apollo/client";
import { SESSIONS } from "./queries";
import SessionItem from "./SessionItem";

function getDay(dayNumber) {
  switch (dayNumber) {
    case 0:
      return "Sunday";
    case 1:
      return "Monday";
    case 2:
      return "Tuesday";
    case 3:
      return "Wednesday";
    case 4:
      return "Thursday";
    case 5:
      return "Friday";
    case 6:
      return "Saturday";
    default:
      throw new Error();
  }
}

export default function SessionList({ sessionDay }) {
  const day = sessionDay ? sessionDay : getDay(new Date().getDay());
  // execute query and store response json
  const { loading, error, data } = useQuery(SESSIONS, {
    variables: { day },
  });

  if (loading) return <p>Loading Sessions..</p>;

  if (error) return <p>Error loading sessions!</p>;

  const results = [];

  results.push(
    data.intro.map((session) => (
      <SessionItem
        key={session.id}
        session={{
          ...session,
        }}
      />
    ))
  );

  results.push(
    data.intermediate.map((session) => (
      <SessionItem
        key={session.id}
        session={{
          ...session,
        }}
      />
    ))
  );

  results.push(
    data.advanced.map((session) => (
      <SessionItem
        key={session.id}
        session={{
          ...session,
        }}
      />
    ))
  );

  return results;
}
