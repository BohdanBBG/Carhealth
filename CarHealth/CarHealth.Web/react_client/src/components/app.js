
import React, { Component } from 'react';

import { Router, Route, NavLink } from 'react-router-dom'
import { createBrowserHistory } from 'history';
import 'bootstrap/dist/css/bootstrap.css'


const history = createBrowserHistory();


class App extends Component {

  state = {
    
  }

  render() {


    const menuEl =
      this.props.routes.map((route, index) => (
        <NavLink
          exact
          className={`nav-item nav-link  ${route.disabled ? "disabled" : ""}`}
          to={route.route}
          key={index}
          activeClassName="active">
          {route.name}
        </NavLink>

      ));



    return (

      <div className="container-fluid  "  >

        <Router history={history}>

          <div className="row ">

            <div className=" pt-4 col-sm-2 nav nav-pills text-center d-flex flex-column    pl-3" >

              {menuEl}

            </div>

            <div className="col-sm pt-3 container-fluid " >

              {this.props.routes.map((route, index) => (
                <Route exact path={route.route} component={route.component} key={index} />
              ))}

            </div>

          </div>

        </Router>

      </div>

    );
  }
}

export default App;