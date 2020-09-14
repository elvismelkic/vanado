import React, { Fragment, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import FailureStatus from "./FailureStatus";
import DeleteModal from "../DeleteModal";
import ModalContainer from "../ModalContainer";
import { fetchOne, deleteOne, upload } from "../../utils/api/calls";

export default function FailurePage(props) {
  const id = props.match.params.id;
  const [failure, setFailure] = useState({});
  const [loading, setLoading] = useState(true);
  const [isDeleteModalActive, setIsDeleteModalActive] = useState(false);
  const [isImageModalActive, setIsImageModalActive] = useState(false);
  const [isFileExistsModalActive, setIsFileExistsModalActive] = useState(false);
  const [filesToUpload, setFilesToUpload] = useState([]);
  const [images, setImages] = useState([]);
  const [documents, setDocuments] = useState([]);
  const [modalImage, setModalImage] = useState();

  useEffect(() => {
    async function fetchData() {
      const failure = await fetchOne("failures", id);
      setFailure(failure);
      setImages(failure.files.filter((file) => file.type.includes("image/")));

      setDocuments(
        failure.files.filter((file) => !file.type.includes("image/"))
      );

      setLoading(false);
    }

    fetchData();
  }, [id]);

  const handleUploadSubmit = async (event) => {
    if (event) event.preventDefault();

    const response = await upload(filesToUpload, id);

    if (response.message) setIsFileExistsModalActive(true);
  };

  return loading ? (
    <h2 className="page-title">Loading failure...</h2>
  ) : failure.errors ? (
    <h2 className="page-title">Failure not found.</h2>
  ) : (
    <Fragment>
      <FailureDetails
        id={id}
        failure={failure}
        setFailure={setFailure}
        setIsDeleteModalActive={setIsDeleteModalActive}
      />
      <ImagesContainer
        images={images}
        setImages={setImages}
        setModalImage={setModalImage}
        setIsImageModalActive={setIsImageModalActive}
      />
      <DocumentsContainer documents={documents} setDocuments={setDocuments} />
      <UploadFiles
        handleUploadSubmit={handleUploadSubmit}
        setFilesToUpload={setFilesToUpload}
      />
      <ModalContainer
        isModalActive={isDeleteModalActive}
        setIsModalActive={setIsDeleteModalActive}
      >
        <DeleteModal
          setIsModalActive={setIsDeleteModalActive}
          deleteApiCall={deleteOne}
          id={failure.id}
          route="failures"
          target="failure"
        />
      </ModalContainer>
      <ModalContainer
        isModalActive={isImageModalActive}
        setIsModalActive={setIsImageModalActive}
      >
        <div className="modal__container--img">
          <img src={modalImage} alt="Modal" />
        </div>
      </ModalContainer>
      <ModalContainer
        isModalActive={isFileExistsModalActive}
        setIsModalActive={setIsFileExistsModalActive}
      >
        <FileExistsModal setIsModalActive={setIsFileExistsModalActive} />
      </ModalContainer>
    </Fragment>
  );
}

const FailureDetails = ({
  id,
  failure,
  setFailure,
  setIsDeleteModalActive,
}) => {
  return (
    <Fragment>
      <h2 className="page-title">{failure.name}</h2>
      <p className="failure__field">
        <span className="failure__field--span">Machine:</span>
        <Link to={`/machines/${failure.machineId}`}>{failure.machineName}</Link>
      </p>
      <div className="failure__field failure__field--flex">
        <span className="failure__field--span">Status:</span>
        <FailureStatus
          failure={failure}
          setFailure={setFailure}
          addSpace={true}
        />
      </div>
      <p className="failure__field">
        <span className="failure__field--span">Priority:</span>
        <span
          className={`item__text--priority item__text--${failure.priority}`}
        >
          {failure.priority}
        </span>
      </p>
      <p className="failure__field">
        <span className="failure__field--span">Description:</span>
        {failure.description || "No additional description was provided."}
      </p>
      <Link className="btn btn--edit btn--link" to={`/failures/${id}/edit`}>
        Edit
      </Link>
      <button
        className="btn btn--delete"
        onClick={() => {
          setIsDeleteModalActive(true);
        }}
      >
        Delete
      </button>
    </Fragment>
  );
};

const ImagesContainer = ({
  images,
  setImages,
  setModalImage,
  setIsImageModalActive,
}) => {
  const handleDelete = async (image) => {
    const remainingFiles = await deleteOne("files", image.id);

    setImages(remainingFiles.filter((file) => file.type.includes("image/")));
  };

  const handleClick = (image) => {
    setModalImage(`/images/failure_${image.failureId}/${image.name}`);
    setIsImageModalActive(true);
  };

  return (
    <div className="failure__field">
      <span>Images:</span>
      {images.length === 0 ? (
        <p>No images were uploaded.</p>
      ) : (
        <ul className="failure__images">
          {images.map((image) => {
            return (
              <li key={image.id} className="failure__img-container">
                <button
                  type="button"
                  className="failure__file-btn failure__file-btn--img failure__file-btn--delete"
                  onClick={() => {
                    handleDelete(image);
                  }}
                >
                  X
                </button>
                <img
                  src={`/images/failure_${image.failureId}/${image.name}`}
                  alt="Failure"
                  className="failure__img"
                  onClick={() => {
                    handleClick(image);
                  }}
                />
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};

const DocumentsContainer = ({ documents, setDocuments }) => {
  return (
    <div className="failure__field">
      <span>Documents:</span>
      {documents.length === 0 ? (
        <p>No documents were uploaded.</p>
      ) : (
        <ul>
          {documents.map((document) => {
            return (
              <li key={document.id} className="failure__document">
                <span>{document.name}</span>
                <Link
                  to={`/images/failure_${document.failureId}/${document.name}`}
                  className="failure__file-btn failure__file-btn--download"
                  target="_blank"
                  download
                >
                  &darr;
                </Link>
                <button
                  type="button"
                  className="failure__file-btn failure__file-btn--delete"
                  onClick={async () => {
                    const remainingFiles = await deleteOne(
                      "files",
                      document.id
                    );

                    setDocuments(
                      remainingFiles.filter(
                        (file) => !file.type.includes("image/")
                      )
                    );
                  }}
                >
                  X
                </button>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};

const UploadFiles = ({ handleUploadSubmit, setFilesToUpload }) => {
  const [numberOfFiles, setNumberOfFiles] = useState(0);

  return (
    <form className="form" onSubmit={handleUploadSubmit}>
      <div className="form__field form__field--upload">
        <label className="form__label">
          <span className="form__field-name form__field-name--upload">
            Upload images or documents (max. 5 files):
          </span>
          <input
            className="form__upload-input"
            type="file"
            onChange={(event) => {
              const files = event.target.files;

              setNumberOfFiles(files.length);
              setFilesToUpload(files);
            }}
            id="file"
            name="file"
            accept="audio/*,video/*,image/*,application/pdf,application/doc,application/docx"
            multiple
          />
        </label>
        <button
          className="btn btn--edit btn--upload"
          type="submit"
          disabled={numberOfFiles === 0 || numberOfFiles > 5}
        >
          Upload
        </button>
      </div>
    </form>
  );
};

const FileExistsModal = ({ setIsModalActive }) => {
  const handleClick = () => {
    setIsModalActive(false);
  };

  return (
    <div className="modal__container--text">
      <p>One of the files already exists.</p>
      <div className="modal__btn-container">
        <button className="modal__btn modal__btn--blue" onClick={handleClick}>
          Ok
        </button>
      </div>
    </div>
  );
};
