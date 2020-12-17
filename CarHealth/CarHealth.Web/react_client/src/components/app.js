
import React, { Component } from 'react';

import { Router, Route, NavLink } from 'react-router-dom';

import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import ModalWindow from '../components/forms/ModalWindow.js';
import HomePage from '../components/routes/homePage/HomePage.js';

import { AuthService } from "../services/AuthService.js";

import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { changeFirstName, changeSecondName } from '../store/examples/actions.js';
import { setAppConfig, setAccessToken } from '../store/auth/actions.js';

import { createBrowserHistory } from 'history';

import 'bootstrap/dist/css/bootstrap.css';
import "../styles/css/sb-admin-2.min.css";

const history = createBrowserHistory();


class App extends Component {

  authService;

  constructor(props) {
    super(props);

    this.authService = new AuthService(props.config.auth);

    this.props.setAppConfig(props.config);

  }

  logout = () => {
    this.authService.logout();
  }

  getUser = () => {

    this.authService.getUser().then(user => {

      if (user) {

        this.props.setAccessToken(user.access_token);

        console.log('User logged on:', user);
        //console.log('0-0-0-0-0-0 ', this.props.accessToken);

      } else {

        console.info('You are not logged in.');

        this.authService.login();
      }

    });
  }

  componentDidMount() {

    this.getUser();

    console.log("App ----", ' did mount');
  }

  componentWillUnmount() {
    this.shouldCancel = true;
  }

  getToast = () => {

    toast.success("Success Notification !", {
    });

    // toast.error("Error Notification !", {
    // });

    // toast.warn("Warning Notification !", {
    // });

    // toast.info("Info Notification !", {
    // });

  }


  render() {

    const menuEl =
      this.props.routes.map((route) => (
        <NavLink
          exact
          className={`nav-item nav-link ${route.disabled ? "disabled" : ""}`}
          to={route.route}
          key={route.name}
          onClick={this.getToast}
          activeClassName="active">
          <i className="fas fa-fw fa-cog"></i>
          {route.name}
        </NavLink>
      ));

    const { changeFirstName, changeSecondName } = this.props;

    return (
      <div>

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

                    {this.props.routes.map((route) => (
                      <Route exact path={route.route} component={route.component} urls={this.props.appConfig} key={route.name} />
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
              <div className="modal-body">Select "Logout" below if you are ready to end your current session.</div>
              <div className="modal-footer">
                <button className="btn btn-secondary text-white" type="button" data-dismiss="modal">Cancel</button>
                <a className="btn btn-primary" onClick={this.logout}>Logout</a>
              </div>
            </ModalWindow>

          </Router>

          <ToastContainer
            position="top-right"
            autoClose={5000}
            hideProgressBar={false}
            newestOnTop
            closeOnClick
            rtl={false}
            pauseOnFocusLoss
            draggable
            pauseOnHover
          />

        </div>

        <div>
          <input
            type="text"
            value={this.props.firstName}
            placeholder="First Name"
            onChange={(event) => {
              changeFirstName(event.target.value);
            }}
          ></input>
        </div>
        <div>
          <input
            type="text"
            value={this.props.secondName}
            placeholder="Second Name"
            onChange={(event) => {
              changeSecondName(event.target.value);
            }}
          ></input>
        </div>

        <div>{this.props.firstName + ' ' + this.props.secondName}</div>
      </div>);
  }
}

const putStateToProps = (state) => {

  return {
    firstName: state.example.firstName,
    secondName: state.example.secondName,
    accessToken: state.config.accessToken,
    appConfig: state.config.appConfig
  };
}
const putActionsToProps = (dispatch) => {

  return {
    changeFirstName: bindActionCreators(changeFirstName, dispatch),
    changeSecondName: bindActionCreators(changeSecondName, dispatch),

    setAccessToken: bindActionCreators(setAccessToken, dispatch),
    setAppConfig: bindActionCreators(setAppConfig, dispatch),
  };

};

export default connect(putStateToProps, putActionsToProps)(App);