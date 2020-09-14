import React, { Fragment, useState, useEffect, useRef } from "react";
import { toggleFailureStatus } from "../../utils/api/calls";

export default function FailureStatus({ failure, setFailure, addSpace }) {
  const [status, setStatus] = useState(failure.isFixed);
  const prevStatusRef = useRef();

  useEffect(() => {
    async function changeStatus(id) {
      const status = await toggleFailureStatus(id);

      setFailure({ ...failure, isFixed: status });
    }

    prevStatusRef.current = status;

    if (prevStatus !== status) {
      changeStatus(failure.id);
    }
  });

  const prevStatus = prevStatusRef.current || failure.isFixed;

  return (
    <Fragment>
      <p
        className={`failure__status ${status ? "failure__status--fixed" : ""} ${
          addSpace ? "failure__status--space" : ""
        }`}
      >
        {status ? "Fixed" : "Not fixed"}
      </p>
      <div className="failure-item__btn-container">
        <button
          className="failure__btn"
          onClick={() => {
            setStatus(!status);
          }}
        >
          Change Status
        </button>
      </div>
    </Fragment>
  );
}
