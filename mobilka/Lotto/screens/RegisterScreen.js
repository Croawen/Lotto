import React from 'react';
import {View, StyleSheet,TextInput,Image,Text} from 'react-native'
import BlueButton from '../components/BlueButton';
import ApiManager from '../helper/ApiManager';
import Preferences from '../helper/Preferences';
import colors from '../helper/colors';

export default class RegisterScreen extends React.Component {
    
  state ={
    login:"",
    password:""
  }

  loginClick = async () => {
    if(this.state.login != "" && this.state.password != ""){
       response = await ApiManager.register(this.state.login, this.state.password)
        if(response != undefined){
          await Preferences.put(Preferences.USER, response)
          this.props.reload()
        }
    }
  }

  render() {
    return (
        <View style ={styles.container}>

            <Image source={require("../assets/loginBanner.png")} style={{height: 120,width:140}}/>
            <TextInput
              style={{marginTop: 34, height: 50,width:250, borderColor: colors.colorAccent, borderWidth: 1, padding: 14, textAlign:"center"}}
              onChangeText={(text) => this.setState({login:text})}
              value={this.state.login}
              placeholder={"e-mail"}
            />
            <TextInput
              style={{height: 50,width:250, borderColor: colors.colorAccent, borderWidth: 1, marginTop:12, padding: 14, textAlign:"center"}}
              onChangeText={(text) => this.setState({password:text})}
              value={this.state.password}
              placeholder={"hasło"}
            />
            
            
            <BlueButton style={styles.login} name="Zaloguj" buttonWidth={250} onButtonPress={this.loginClick}/>

            <Text style={[styles.text, {fontSize:16, marginTop:6}]}>lub <Text style={{color:colors.colorAccent}}>Stwórz konto</Text></Text>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems:"center",
    justifyContent:"center"
  },
  login:{
    marginTop:12
  }
});