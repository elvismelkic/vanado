import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import FailureStatus from "./FailureStatus";

export default function FailureListItem({
  failure: passedFailure,
  showMachine,
}) {
  const [failure, setFailure] = useState(passedFailure);

  return (
    <div className="failure__item">
      <p className="item__text">{failure.name}</p>
      {showMachine ? <p className="item__text">{failure.machineName}</p> : null}
      <FailureStatus failure={failure} setFailure={setFailure} />
      <p
        className={`item__text item__text--priority item__text--${failure.priority}`}
      >
        {failure.priority}
      </p>
      <NavLink className="item__text--details" to={`/failures/${failure.id}`}>
        Details
      </NavLink>
    </div>
  );
}
