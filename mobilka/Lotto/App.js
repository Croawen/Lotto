import React from 'react';
import {View, StyleSheet, Platform} from 'react-native'
import AppNavigator from './components/AppNavigator';
import colors from './helper/colors';
import LoginScreen from './screens/LoginScreen';
import Preferences from './helper/Preferences';


export default class App extends React.Component {

  state ={
    isLogged: undefined
  }

  componentWillMount(){
      this.reloadView()
  }

  reloadView=async()=>{
    user = await Preferences.get(Preferences.USER)
    this.setState({
      isLogged: user!=undefined
    })
  }

  render() {
    return (
      <View style={{flex:1}}>
        <View style={{height:Platform.OS=="ios"?20:25, backgroundColor:colors.colorAccent}}/>
        {this.state.isLogged != undefined && (this.state.isLogged ? <AppNavigator/> : <LoginScreen reload={this.reloadView}/>)}
      </View>
    );
  }
}