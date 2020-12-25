
import React, { Component, Children } from 'react'
import 'bootstrap/dist/css/bootstrap.css'

class Form extends Component {

    componentDidMount() {
        console.log(`Form----${this.props.id} did mount`);
    }

    constructor(props) {
        super(props);
        this.state = { login: '', password: '' };

        this.onChangeLogin = this.onChangeLogin.bind(this);
        this.onChangePassword = this.onChangePassword.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(event) {
        const children = React.Children.toArray(this.props.children);
        const child = React.Children.toArray(children[0].props.children);
        console.log(child[1].props.name);

        this.setState({ [child[1].props.name]: child[1].props.value })
        console.log(this.state);
        event.preventDefault();
    }

    onChangePassword(event) {
        this.setState({ password: event.target.value });
    }

    onChangeLogin(event) {
        this.setState({ login: event.target.value });
    }

    // render() {
    //     return (
    //         <form onSubmit={this.onSubmit}>
    //            {this.props.children}
    //         </form>
    //     );
    // }


    render() {

        return (
            <form onSubmit={this.onSubmit} id={this.props.id}  >

                {this.renderChildren()}

                <div className="row justify-content-end ">
                    <button type="button" className="btn btn-secondary col-2" data-dismiss="modal">Close</button>
                    <button type="submit" className="btn btn-primary col-2 ml-2">Save</button>
                </div>

            </form>
        );
    }

    onSubmit = (event) => {
        //alert(Children.count(this.props.children));        
        const children = React.Children.toArray(this.props.children)
        alert(children[1]);
    }

    renderChildren() {
        return Children.map(this.props.children, child => {
            return React.cloneElement(child, {
                name: "this.props.name"
            })
        })
    }
}

export default Form; 