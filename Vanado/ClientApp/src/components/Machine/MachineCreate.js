import React, { Fragment, useState } from "react";
import MachineForm from "./MachineForm";
import ModalContainer from "../Modal/ModalContainer";
import SomethingWentWrongModal from "../Modal/SomethingWentWrongModal";
import { addOne } from "../../utils/api/calls";

export default function NewMachine({ history }) {
  const [inputData, setInputData] = useState({ name: "" });
  const [isModalActive, setIsModalActive] = useState(false);

  const redirect = (createdMachineId) => {
    const redirectRoute = createdMachineId
      ? `/machines/${createdMachineId}`
      : "/";

    history.push(redirectRoute);
  };

  const handleSubmit = async (event) => {
    if (event) event.preventDefault();

    const response = await addOne("machines", inputData);

    if (response.message) return setIsModalActive(true);

    const createdMachineId = response;

    redirect(createdMachineId);
  };

  const handelCancel = () => {
    redirect();
  };

  return (
    <Fragment>
      <h2 className="page-title">Add new machine</h2>
      <MachineForm
        handleSubmit={handleSubmit}
        handleCancel={handelCancel}
        inputData={inputData}
        setInputData={setInputData}
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
