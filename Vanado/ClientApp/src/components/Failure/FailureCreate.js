import React, { Fragment, useState, useEffect } from "react";
import { fetchAll, addOne } from "../../utils/api/calls";
import { parse } from "query-string";
import FailureForm from "./FailureForm";
import ModalContainer from "../Modal/ModalContainer";
import SomethingWentWrongModal from "../Modal/SomethingWentWrongModal";

export default function FailureCreate({ history, location }) {
  const machineId = parseInt(parse(location.search).machine);
  const [isModalActive, setIsModalActive] = useState(false);

  const [inputData, setInputData] = useState({
    name: "",
    isFixed: false,
    priority: "moderate",
  });

  const [machines, setMachines] = useState([]);

  useEffect(() => {
    async function fetchData() {
      const machines = await fetchAll("machines");

      if (machines.length > 0) {
        setMachines(machines);
        setInputData((currentInputData) => ({
          ...currentInputData,
          machineId: machineId || machines[0].id,
        }));
      }
    }

    fetchData();
  }, [machineId]);

  const redirect = (createdFailureId) => {
    const redirectRoute = machineId
      ? `/machines/${machineId}`
      : createdFailureId
      ? `/failures/${createdFailureId}`
      : "/";

    history.push(redirectRoute);
  };

  const handleSubmit = async (event) => {
    if (event) event.preventDefault();

    const response = await addOne("failures", inputData);

    if (response.message) return setIsModalActive(true);

    const createdFailureId = response;

    redirect(createdFailureId);
  };

  const handelCancel = () => {
    redirect();
  };

  return machines.length === 0 ? (
    <h2 className="page-title">There is no machine to add failure to.</h2>
  ) : (
    <Fragment>
      <h2 className="page-title">Add new failure</h2>
      <FailureForm
        handleSubmit={handleSubmit}
        handleCancel={handelCancel}
        machines={machines}
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
