import React from "react";
import FailureListItem from "./FailureListItem";

export default function FailuresList({ failures, showMachine }) {
  return (
    <ul>
      <li>
        <div className="failure__item">
          <p className="item__text item__text--header">Failure name</p>
          {showMachine ? (
            <p className="item__text item__text--header">Machine</p>
          ) : null}
          <p className="item__text item__text--header">Status</p>
        </div>
      </li>
      {failures ? (
        failures.map(failure => (
          <li key={failure.id}>
            <FailureListItem failure={failure} showMachine={showMachine} />
          </li>
        ))
      ) : (
        <div />
      )}
    </ul>
  );
}
