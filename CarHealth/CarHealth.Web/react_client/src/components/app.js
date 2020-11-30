
import React, { Component } from 'react';

import { Router, Route, NavLink } from 'react-router-dom';
import { createBrowserHistory } from 'history';

import ModalWindow from "../components/forms/ModalWindow.js"
import HomePage from '../components/routes/homePage/HomePage.js'

import AuthService from "../services/AuthService.js"

import 'bootstrap/dist/css/bootstrap.css';
import "../styles/css/sb-admin-2.min.css";

const history = createBrowserHistory();

class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      webConfig: this.props.appConfig,
      authService: new AuthService(this.props.appConfig),
    }
  }

  logout = () => {
    this.state.authService.logout();
  }

  componentDidMount() {

    this.state.authService.getUser((user) => {

      if (user) {
        console.log('User logged on:', user);
      } else {
        console.log("User not logged in");

        this.state.authService.login();
      }
    });

    console.log("CarPartsPage----", ' did mount');
  }

  render() {

    const menuEl =
      this.props.routes.map((route, index) => (
        <NavLink
          exact
          className={`nav-item nav-link ${route.disabled ? "disabled" : ""}`}
          to={route.route}
          key={index + 1}
          activeClassName="active">
          <i className="fas fa-fw fa-cog"></i>
          {route.name}
        </NavLink>

      ));

    return (

      <div id="wrapper">
        <Router history={history}>

          <ul className="navbar-nav bg-gray-900 sidebar sidebar-dark accordion" id="accordionSidebar">

            <div className="sidebar-brand d-flex align-items-center justify-content-center">
              <p className="sidebar-brand-text mx-3">Welcome</p>
            </div>

            <li className="nav-item">
              <NavLink
                exact
                className={"nav-item nav-link"}
                to={"/"}
                key={0}
                activeClassName={"active"}>
                <i className="fas fa-fw fa-cog"></i>
                {"Home"}
              </NavLink>

              {menuEl}
            </li>

          </ul>

          <div id="content-wrapper" className="d-flex flex-column">

            <div id="content">

              <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

                <ul className="navbar-nav ml-auto">

                  <div className="topbar-divider d-none d-sm-block"></div>

                  <li className="nav-item dropdown no-arrow " >
                    <a className="nav-link text-dark" data-toggle="modal" href="#logout">
                      <i className="fas fa-sign-out-alt fa-sm fa-fw mr-2 "></i>
                                    Logout
                    </a>

                  </li>

                </ul>

              </nav>

              <div className="container-fluid">

                <Route exact path={"/"} component={HomePage} key={0} />

                <div className="card shadow mb-4 border-0">

                  {this.props.routes.map((route, index) => (
                    <Route exact path={route.route} component={route.component} key={index + 1} />
                  ))}

                </div>

              </div>

            </div>

            <footer className="sticky-footer ">
              <div className="container my-auto">
                <div className="copyright text-center my-auto">
                  <span>Copyright &copy; CarHealth 2021</span>
                </div>
              </div>
            </footer>

          </div>

          <ModalWindow title="Ready to Leave?" id="logout" inCenter={false}>
            <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
            <div class="modal-footer">
              <button class="btn btn-secondary text-white" type="button" data-dismiss="modal">Cancel</button>
              <a class="btn btn-primary" onClick={this.logout}>Logout</a>
            </div>
          </ModalWindow>

        </Router>

      </div>

    );
  }
}

export default App;