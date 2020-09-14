import React from "react";

export default function MachineForm({
  inputData,
  setInputData,
  handleSubmit,
  handleCancel,
}) {
  const handleInputDataChange = (event) => {
    setInputData({
      ...inputData,
      [event.target.id]: event.target.value,
    });
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <label className="form__field">
        <span className="form__field-name">Machine:</span>
        <input
          type="text"
          className="form__input"
          placeholder="New machine name"
          id="name"
          value={inputData.name}
          onChange={handleInputDataChange}
          required
        />
      </label>
      <div className="form__field">
        <button type="submit" className="btn btn--add">
          Submit
        </button>
        <button onClick={handleCancel} className="btn btn--delete">
          Cancel
        </button>
      </div>
    </form>
  );
}
