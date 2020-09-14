import React from "react";

export default function ModalContainer({ isModalActive, setIsModalActive, children }) {
  const handleCancel = event => {
    if (event.target.className.includes("modal__dark-background")) setIsModalActive(false)
  };

  return (
    <div className="modal">
      {/* Darken the background when the modal appears.
          onClick closes opened modal when background is clicked. */}
      <div
        className={`modal__dark-background ${
          isModalActive ? "modal__dark-background--active" : ""
        }`}
        onClick={handleCancel}
      >
        <div className={`modal__container ${
          isModalActive ? "modal__container--active" : ""
        }`}>
          {children}
        </div>
      </div>
    </div>
  );
}
