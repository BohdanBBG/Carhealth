
import React, { Component, PureComponent } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

import ListItem from './ListItem.js'



class CarPartList extends Component {

    componentDidMount() {
        console.log("CarParts----", ' did mount');
    }

    render() {

        const listElements = this.props.data.map((item, index) =>
            <ListItem
                carItem={item}
                key={index}>

            </ListItem>);

        return (
            <div className="container-fluid " >

                <div className="container-fluid  d-flex flex-row flex-wrap pb-2  " >
                    
                    {listElements}
                    {listElements}

                    {listElements}
                    {listElements}

                </div>


            </div>);
    }

}

export default CarPartList