import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import MachinesList from "./Machine/MachinesList";
import MachinePage from "./Machine/MachinePage";
import MachineCreate from "./Machine/MachineCreate";
import FailureCreate from "./Failure/FailureCreate";
import Home from "./Home";
import FailurePage from "./Failure/FailurePage";
import Nav from "./Nav";
import FailureEdit from "./Failure/FailureEdit";
import MachineEdit from "./Machine/MachineEdit";
import NonExistingRoute from "./NonExistingRoute";

export default function App() {
  return (
    <Router>
      <Nav />
      <div className="main">
        <Switch>
          <Route path="/" exact component={Home} />
          <Route path="/machines" exact component={MachinesList} />
          <Route path="/machines/new" component={MachineCreate} />
          <Route path="/machines/:id" exact component={MachinePage} />
          <Route path="/machines/:id/edit" component={MachineEdit} />
          <Route path="/failures/new" component={FailureCreate} />
          <Route path="/failures/:id" exact component={FailurePage} />
          <Route path="/failures/:id/edit" component={FailureEdit} />
          <Route component={NonExistingRoute} />
        </Switch>
      </div>
    </Router>
  );
}
