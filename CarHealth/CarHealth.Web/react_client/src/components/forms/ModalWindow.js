import React, { Component } from 'react'
import 'bootstrap/dist/css/bootstrap.css'


class ModalWindow extends Component {


    render() {

        return (
            <div className="modal fade" id={this.props.id} role="dialog" aria-hidden="true">
                <div className={`modal-dialog ${this.props.inCenter ? "modal-dialog-centered" : ""}`}>
                    <div className="modal-content">

                        <div className="modal-header">
                            <h5 className="modal-title" id="modalWindowTitle" >{this.props.title}</h5>
                            <button type="button" className="close" data-dismiss="modal" >
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            {this.props.children}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default ModalWindow;