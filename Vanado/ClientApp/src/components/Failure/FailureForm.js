import React, { useState, useEffect } from "react";

export default function FailureForm({
  inputData,
  setInputData,
  machines,
  handleSubmit,
  handleCancel,
}) {
  const [showDescriptionMessage, setShowDescriptionMessage] = useState(false);

  useEffect(() => {
    if (inputData.name.length <= 20 && inputData.description?.length > 0) {
      setShowDescriptionMessage(true);
    } else if (inputData.name.length > 20) {
      setShowDescriptionMessage(false);
    } else if (inputData.description?.length === 0) {
      setInputData((currentInputData) => {
        delete currentInputData.description;

        return currentInputData;
      });
      setShowDescriptionMessage(false);
    }
  });

  const handleInputDataChange = (event) => {
    if (event.target.id === "machine") {
      setInputData({ ...inputData, machineId: parseInt(event.target.value) });
    } else if (event.target.id === "isFixed") {
      setInputData({ ...inputData, isFixed: event.target.value === "true" });
    } else {
      setInputData({ ...inputData, [event.target.id]: event.target.value });
    }
  };

  return machines.length === 0 ? (
    <h2 className="page-title">There is no machine to add failure to.</h2>
  ) : (
    <form className="form" onSubmit={handleSubmit}>
      <label className="form__field">
        <span className="form__field-name">Failure name:</span>
        <input
          type="text"
          className="form__input"
          placeholder="Failure Name"
          id="name"
          onChange={handleInputDataChange}
          value={inputData.name}
          required
        />
      </label>
      <label className="form__field">
        <span className="form__field-name">Machine:</span>
        <select
          className="form__select"
          id="machine"
          onChange={handleInputDataChange}
          value={inputData.machineId}
        >
          {machines.map((machine) => (
            <option key={machine.id} value={machine.id}>
              {machine.name}
            </option>
          ))}
        </select>
      </label>
      <label className="form__field">
        <span className="form__field-name">Status:</span>
        <select
          className="form__select"
          id="isFixed"
          onChange={handleInputDataChange}
          value={inputData.isFixed}
        >
          <option key={"false"} value={false}>
            Not Fixed
          </option>
          <option key={"true"} value={true}>
            Fixed
          </option>
        </select>
      </label>
      <label className="form__field">
        <span className="form__field-name">Priority:</span>
        <select
          className="form__select"
          id="priority"
          onChange={handleInputDataChange}
          value={inputData.priority}
        >
          <option key="high" value="high">
            High
          </option>
          <option key="moderate" value="moderate">
            Moderate
          </option>
          <option key="low" value="low">
            Low
          </option>
        </select>
      </label>
      <label className="form__field form__field--textarea">
        <p className="form__field-name form__field-name--textarea">
          Failure description:
          {showDescriptionMessage ? (
            <span className="form__textarea-warning">
              Failure name has to be longer than 20 characters if you want to
              add description.
            </span>
          ) : null}
        </p>
        <textarea
          type="text"
          className="form__textarea"
          placeholder="Failure Description"
          id="description"
          onChange={handleInputDataChange}
          value={inputData.description || ""}
          disabled={inputData.name.length <= 20}
        />
      </label>
      <div className="form__field">
        <button
          type="submit"
          className="btn btn--add"
          disabled={showDescriptionMessage}
        >
          Submit
        </button>
        <button onClick={handleCancel} className="btn btn--delete">
          Cancel
        </button>
      </div>
    </form>
  );
}
