import React from "react";

export default function SomethingWentWrongModal({ setIsModalActive }) {
  const handleClick = () => {
    setIsModalActive(false);
  };

  return (
    <div className="modal__container--text">
      <p>Something went wrong. Please, try again.</p>
      <div className="modal__btn-container">
        <button className="modal__btn modal__btn--blue" onClick={handleClick}>
          Ok
        </button>
      </div>
    </div>
  );
}
