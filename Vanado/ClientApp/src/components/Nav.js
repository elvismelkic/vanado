import React from "react";
import { NavLink } from "react-router-dom";

export default function Nav() {
  return (
    <nav className="nav">
      <ul className="nav__list">
        <li className="nav__item">
          <NavLink className="nav__logo" to="/" exact>
            Vanado App
          </NavLink>
        </li>
        <li className="nav__item">
          <NavLink
            className="nav__link"
            activeClassName="nav__link--active"
            to="/machines"
            exact={true}
          >
            Machines
          </NavLink>
        </li>
        <li className="nav__item">
          <NavLink
            className="nav__link"
            activeClassName="nav__link--active"
            to="/machines/new"
          >
            Add Machine
          </NavLink>
        </li>
        <li className="nav__item">
          <NavLink
            className="nav__link"
            activeClassName="nav__link--active"
            to="/failures/new"
          >
            Add Failure
          </NavLink>
        </li>
      </ul>
    </nav>
  );
}
