import React, { Fragment, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import FailuresList from "../Failure/FailuresList";
import DeleteModal from "../DeleteModal";
import ModalContainer from "../ModalContainer";
import { fetchOne, deleteOne } from "../../utils/api/calls";

export default function MachinePage(props) {
  const id = props.match.params.id;
  const [machine, setMachine] = useState({});
  const [loading, setLoading] = useState(true);
  const [isModalActive, setIsModalActive] = useState(false);

  useEffect(() => {
    async function fetchData() {
      const machine = await fetchOne("machines", id);

      setMachine(machine);
      setLoading(false);
    }

    fetchData();
  }, [id]);

  return loading ? (
    <h2 className="page-title">Loading machine...</h2>
  ) : machine.message ? (
    <h2 className="page-title">Machine not found.</h2>
  ) : (
    <Fragment>
      <div className="edit__container">
        <h2 className="page-title">{machine.name}</h2>
        <Link className="btn btn--edit btn--link" to={`/machines/${id}/edit`}>
          Edit
        </Link>
        <button
          className="btn btn--delete"
          onClick={() => {
            setIsModalActive(true);
          }}
        >
          Delete
        </button>
      </div>
      {machine.failures.length === 0 ? (
        <p className="failure__field">This machine has no failures.</p>
      ) : (
        <FailuresList failures={machine.failures} showMachine={false} />
      )}
      <Link
        className="btn btn--edit btn--link"
        to={`/failures/new?machine=${id}`}
      >
        Add failure
      </Link>
      <ModalContainer
        isModalActive={isModalActive}
        setIsModalActive={setIsModalActive}
      >
        <DeleteModal
          setIsModalActive={setIsModalActive}
          deleteApiCall={deleteOne}
          id={machine.id}
          route="machines"
          target="machine"
        />
      </ModalContainer>
    </Fragment>
  );
}
