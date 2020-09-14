import React, { Fragment, useState, useEffect } from "react";
import FailuresList from "./Failure/FailuresList";
import { fetchAll } from "../utils/api/calls";

export default function Home() {
  const [failures, setFailures] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      const failures = await fetchAll("failures", "fixed=false");

      setFailures(failures);
      setLoading(false);
    }

    fetchData();
  }, []);

  return loading ? (
    <h2 className="page-title">Loading failures...</h2>
  ) : (
    <Fragment>
      <h2 className="page-title">Failures:</h2>
      <FailuresList failures={failures} showMachine={true} />
    </Fragment>
  );
}
