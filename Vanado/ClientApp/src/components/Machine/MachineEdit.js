import React, { useState, useEffect, Fragment } from "react";
import MachineForm from "./MachineForm";
import ModalContainer from "../ModalContainer";
import SomethingWentWrongModal from "../SomethingWentWrongModal";
import { fetchOne, editOne } from "../../utils/api/calls";

export default function MachineEdit({ history, match }) {
  const machineId = match.params.id;
  const [loading, setLoading] = useState(true);
  const [editData, setEditData] = useState({});
  const [isModalActive, setIsModalActive] = useState(false);

  useEffect(() => {
    const fetchItem = async () => {
      const machine = await fetchOne("machines", machineId);

      setEditData(machine);
      setLoading(false);
    };

    fetchItem();
  }, [machineId]);

  const redirect = () => {
    history.push(`/machines/${machineId}`);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    const response = await editOne("machines", machineId, editData);

    if (response.message) return setIsModalActive(true);

    redirect();
  };

  const handelCancel = () => {
    redirect();
  };

  return loading ? (
    <div>Loading</div>
  ) : (
    <Fragment>
      <MachineForm
        handleSubmit={handleSubmit}
        handleCancel={handelCancel}
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
