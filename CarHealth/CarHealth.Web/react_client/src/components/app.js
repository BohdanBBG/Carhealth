import React, { Component } from 'react';

import { BrowserRouter, Route, Link } from 'react-router-dom';
import createBrowserHistory from 'history/createBrowserHistory';

import Home from './routes/home/Home.js'
import CarParts from './routes/car_parts/CarParts.js'
import CarsControl from './routes/carsControl/CarsControl.js'
import Admin from './routes/admin/Admin.js'
import LogOut from './routes/logOut/LogOut.js'

const history = createBrowserHistory();


class App extends Component {
  
  render() {
    return (
      <BrowserRouter history={history}>
        <div>
          <ul>
            <li><Link to="/">Home</Link></li>
            <li><Link to="/carParts">Car parts</Link></li>
            <li><Link to="/carsControl">Cars control</Link></li>
            <li><Link to="/admin">Admin</Link></li>
            <li><Link to="/logOut">LogOut</Link></li>
          </ul>
          <hr />
          <Route exact path="/" component={Home}/>                                            
          <Route exact path="/carParts" component={CarParts} />
          <Route path="/carsControl" component={CarsControl} />
          <Route exact path="/admin" component={Admin}/>  
          <Route exact path="/logOut" component={LogOut}/>  
        </div>
      </BrowserRouter>
    );
  }
}

export default App;