import React, { Fragment, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { fetchAll } from "../../utils/api/calls";

export default function MachinesList() {
  const [machines, setMachines] = useState([]);

  useEffect(() => {
    async function fetchData() {
      const machines = await fetchAll("machines");
      setMachines(machines);
    }

    fetchData();
  }, []);

  return (
    <Fragment>
      <h2 className="page-title">Machines:</h2>
      <ul>
        {machines ? (
          machines.map((machine) => (
            <li className="machine" key={machine.id}>
              <Link className="machine__link" to={`/machines/${machine.id}`}>
                {machine.name}
              </Link>
            </li>
          ))
        ) : (
          <div />
        )}
      </ul>
      <Link className="btn btn--edit btn--link" to="/machines/new">
        Add machine
      </Link>
    </Fragment>
  );
}
