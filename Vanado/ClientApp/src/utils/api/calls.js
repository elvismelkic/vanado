import handleError from "./apiErrorHandler";

const mainUrl = "https://localhost:44325/api/";

export async function fetchAll(route, query) {
  const URI = `${mainUrl}${route}${query !== undefined ? "?" + query : ""}`;
  const response = await fetch(URI).catch(handleError);

  return await response.json();
}

export async function fetchOne(route, id) {
  const URI = `${mainUrl}${route}/${id}`;
  const response = await fetch(URI).catch(handleError);

  return await response.json();
}

export async function addOne(route, data) {
  const URI = `${mainUrl}${route}/`;
  const response = await fetch(URI, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  }).catch(handleError);

  return await response.json();
}

export async function toggleFailureStatus(id) {
  const URI = `${mainUrl}failures/${id}`;
  const response = await fetch(URI, {
    method: "PATCH",
    headers: { "Content-Type": "application/json" },
    body: "{}",
  }).catch(handleError);

  return await response.json();
}

export async function deleteOne(route, id) {
  const URI = `${mainUrl}${route}/${id}`;
  const response = await fetch(URI, {
    method: "DELETE",
  }).catch(handleError);

  return await response.json();
}

export async function editOne(route, id, data) {
  const URI = `${mainUrl}${route}/${id}`;
  const response = await fetch(URI, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  }).catch(handleError);

  return await response.json();
}

export async function upload(files, failureId) {
  const data = new FormData();

  for (let i = 0; i < files.length; i++) {
    data.append("files", files[i]);
  }
  data.append("failure", failureId);

  const URI = `${mainUrl}files`;
  const response = await fetch(URI, {
    method: "POST",
    body: data,
  }).catch(handleError);

  return await response.json();
}
