import React, { useState, useEffect, Fragment } from "react";
import { fetchAll, fetchOne, editOne } from "../../utils/api/calls";
import FailureForm from "./FailureForm";
import ModalContainer from "../ModalContainer";
import SomethingWentWrongModal from "../SomethingWentWrongModal";

export default function FailureEdit({ history, match }) {
  const failureId = match.params.id;
  const [loading, setLoading] = useState(true);
  const [editData, setEditData] = useState({});
  const [machines, setMachines] = useState([]);
  const [isModalActive, setIsModalActive] = useState(false);

  useEffect(() => {
    const fetchItem = async () => {
      const failure = await fetchOne("failures", failureId);
      const machines = await fetchAll("machines");

      setMachines(machines);
      setEditData(failure);
      setLoading(false);
    };

    fetchItem();
  }, [failureId]);

  const redirect = () => {
    history.push(`/failures/${failureId}`);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    const response = await editOne("failures", failureId, editData);

    if (response.message) return setIsModalActive(true);

    redirect();
  };

  const handelCancel = () => {
    redirect();
  };

  return loading ? (
    <div>Loading...</div>
  ) : (
    <Fragment>
      <FailureForm
        handleSubmit={handleSubmit}
        handleCancel={handelCancel}
        machines={machines}
        inputData={editData}
        setInputData={setEditData}
      />
      <ModalContainer
        isModalActive={isModalActive}
        setIsModalActive={setIsModalActive}
      >
        <SomethingWentWrongModal setIsModalActive={setIsModalActive} />
      </ModalContainer>
    </Fragment>
  );
}
