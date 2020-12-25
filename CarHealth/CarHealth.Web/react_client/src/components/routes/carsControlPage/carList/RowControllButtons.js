
import React, { Component} from 'react'
import Dropdown from 'react-bootstrap/Dropdown';
import 'bootstrap/dist/css/bootstrap.css'

class RowControllButtons extends Component {

    render() {
        return (

            <Dropdown>
                <Dropdown.Toggle variant="btn-outline-secondar" id="dropdown-basic">
                    <i className="fas fa-cog"></i>
                </Dropdown.Toggle>

                <Dropdown.Menu>
                    <Dropdown.Item data-toggle="modal" href="#carEntityEditModalWindow" onClick={this.props.onEditButtonClick}>
                        <i className="fas fa-pen"></i>
                        <span>Edit </span>
                    </Dropdown.Item>
                    <Dropdown.Item data-toggle="modal" href="#carEntityDeleteModalWindow" onClick={this.props.onEditButtonClick}>
                        <i className="fas fa-trash"></i>
                        <span>Delete </span>
                    </Dropdown.Item>
                </Dropdown.Menu>
            </Dropdown>

        );
    }
}

export default RowControllButtons;