import React from "react";
import { useHistory } from "react-router-dom";

export default function DeleteModal(props) {
  const history = useHistory();

  const handleDelete = async () => {
    await props.deleteApiCall(`${props.route}`, props.id);
    history.push("/");
  };

  const handleCancel = () => {
    props.setIsModalActive(false);
  };

  return (
    <div className="modal__container--text">
      <p>Are you sure you want to delete this {props.target}?</p>
      <div className="modal__btn-container">
        <button className="modal__btn modal__btn--red" onClick={handleDelete}>
          Yes
        </button>
        <button className="modal__btn modal__btn--no" onClick={handleCancel}>
          No
        </button>
      </div>
    </div>
  );
}
