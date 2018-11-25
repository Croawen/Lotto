import React from 'react';
import {View, StyleSheet, Text, StatusBar} from 'react-native'
import { MapView, Location, Permissions } from 'expo';
import mapstyle from '../helper/mapstyle'
import BlueButton from '../components/BlueButton';
import polandcoords from '../helper/polandcoords';
import UserMarker from '../components/UserMarker';
import colors from '../helper/colors';
import CircleMarker from '../components/CircleMarker';
import SocketManager from '../helper/SocketManager';
import Preferences from '../helper/Preferences';
import WinPopUp from './WinPopUp';
import LosePopUp from './LosePopUp';


export default class HomeScreen extends React.Component {
    locationService
    socketManager

    state={
        userCoords:undefined,
        circleCoords:{
            latitude: 0,
            longitude: 0
        },
        step: 0,
        generatorRun: true,
        nextDrawSeconds : 60,
        participants:0,
        rollId: -1,
        winPopup: false,
        losePopup: false,
        lastCircleCoords:{
            latitude: 0,
            longitude: 0
        },
        lastStatus: null
    }

    generateRandomCircleCoords = async () => {
        while(this.state.generatorRun){
            await new Promise(res => setTimeout(res, 1000))
            if(this.state.nextDrawSeconds < 8){
                const random1 = (49 + (Math.random() * (54 - 49)));
                const random2 = (14 + (Math.random() * (22 - 14)));
                this.setState({
                    circleCoords:{
                    latitude: random1,
                    longitude: random2
                    }
                })
            }
            if(this.state.nextDrawSeconds-1 >= 0){
                this.setState({
                nextDrawSeconds: this.state.nextDrawSeconds-1
            })}
            
        }
    }

  componentDidMount() {
    StatusBar.setBarStyle('light-content');
    this.generateRandomCircleCoords()
    this.getLocationAsync();

    this.startSocket()
    

  }

  startSocket=async()=>{
    user = await Preferences.get(Preferences.USER)
    this.socketManager = new SocketManager({id:user.user_id})
    this.socketManager.onChangeListener = (type, message) =>{
        if(type == "NextRollData" ){
            this.setState((previousState, currentProps) => {
                if(previousState.rollId != message.rollId){
                    this.socketManager.sendMessage("InvokeHasWonRoll",previousState.rollId)
                    return {
                        nextDrawSeconds: message.rollDate,
                        participants: message.participantsCount,
                        rollId: message.rollId,
                        lastCircleCoords:this.state.circleCoords,
                        circleCoords:{
                            latitude: 0,
                            longitude: 0
                        },
                        step:0,
                        lastStatus: null,
                        
                    };
                } else {
                    return {
                        nextDrawSeconds: message.rollDate,
                        participants: message.participantsCount
                    };
                }
            });

            



        }else if(type == "HasWonRoll" ){
            this.setState({
                winPopup: message != null && message,
                losePopup: message != null && !message,
            })
        }


        
    }
    
  }

  componentWillUnmount(){
    this.setState({generatorRun:false})
    this.removeLocationService()
  }

  removeLocationService = () =>{
    if(this.locationService!=undefined && this.locationService!=null)
        this.locationService.remove()
  }

  playClick = () => {
    this.socketManager.sendMessage("InvokeBuyTicket", 
        {RollId: this.state.rollId, Longitude: this.state.userCoords.longitude, Latitude: this.state.userCoords.latitude})
    this.setState({
        step:1
    })
  }

  getLocationAsync = async () => {
    let { status } = await Permissions.askAsync(Permissions.LOCATION);
    if (status !== 'granted') {
      return
    }
    this.removeLocationService()
    this.locationService = await Location.watchPositionAsync({}, 
        (callback)=> {
            this.setState({userCoords: callback!= undefined &&callback.coords != undefined ? callback.coords: undefined})
        })

  };


  render() {


    return (
        <View style ={styles.container}>
            <MapView
                provider="google"
                customMapStyle={mapstyle}
                style ={styles.map}
                showsUserLocation={false}
                showsMyLocationButton={false}
                zoomEnabled={false}
                showsCompass={false}
                zoomControlEnabled={false}
                rotateEnabled={false}
                scrollEnabled={false}
                pitchEnabled={false}
                initialRegion={polandcoords}>
                {this.state.userCoords != undefined && (
                    <UserMarker
                        coordinate={{
                            latitude: this.state.userCoords.latitude,
                            longitude: this.state.userCoords.longitude}}
                    />
                )}
                {this.state.circleCoords != undefined && (
                    <CircleMarker
                        coordinate={this.state.circleCoords}
                    />
                )}
            </MapView>

            {this.state.step == 0 && (
                <View style={styles.headerContainer}>
                    <Text style={[styles.text, {fontSize:16}]}>Zlokalizuj szczęście!</Text>
                    <Text style={[styles.text, {fontSize:12}]}>Pula na tą grę wynosi:</Text>
                    <Text style={[styles.text, {fontSize:36, color: colors.colorAccent, marginTop:12}]}>100 000zł</Text>
                </View>
            )}

            {(this.state.step == 0  || this.state.step == 1) && (
                <View style={styles.timeCointainer}>
                    <Text style={[styles.text, {fontSize:16}]}>Następne losowanie za:</Text>
                    <Text style={[styles.text, {fontSize:36}]}>{this.state.nextDrawSeconds}s</Text>
                </View>
            )}

            {(this.state.step == 1 || this.state.step == 3) && (
                <View style={styles.headerContainer}>
                    <Text style={[styles.text, {fontSize:24}]}>{this.state.step == 1 ? "Witaj w grze!" : "Gra w toku!"}</Text>
                    <View style={{flexDirection:"row", marginTop: 12}}>
                        <View style={{alignItems:"center"}}>
                            <Text style={[styles.text, {fontSize:20, color: colors.colorAccent}]}>100 000zł</Text>
                            <Text style={[styles.text, {fontSize:12}]}>Kwota w puli</Text>
                        </View>
                        <View style={{alignItems:"center", marginLeft: 32}}>
                            <Text style={[styles.text, {fontSize:20, color: colors.colorAccent}]}>{this.state.participants}</Text>
                            <Text style={[styles.text, {fontSize:12}]}>Aktywnych graczy</Text>
                        </View>
                    </View>
                </View>
            )}

            {(this.state.step == 1 || this.state.step == 3)&& (
                <View style={styles.bottomCoords}>
                    <Text style={styles.circleCoords}>{this.state.circleCoords.latitude == 0 ? "-":
                        `${this.state.circleCoords.latitude.toFixed(4)} E`}</Text>
                    <Text style={styles.circleCoords}>{this.state.circleCoords.longitude == 0 ? "-":
                        `${this.state.circleCoords.longitude.toFixed(4)} N`}</Text>
                </View>
            )}

            {this.state.step == 0 && (
                <BlueButton style={styles.play} name="GRAJ" buttonWidth={150} onButtonPress={this.playClick}/>
            )}

            {this.state.winPopup && (
            <WinPopUp playAgainClick={()=>{this.setState({winPopup:false,losePopup:false})}}/>)}

            {this.state.losePopup && (
            <LosePopUp coordinates={this.state.lastCircleCoords} 
            timeleft={this.state.nextDrawSeconds} playAgainClick={()=>{this.setState({winPopup:false,losePopup:false})}}/>)}
        </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent:"center"
  },

  map:{
    flex: 1,
  },
  headerContainer:{
    alignSelf:"center",
    alignItems:"center",
    position:"absolute",
    top:21
  },
  timeCointainer:{
    alignSelf:"center",
    alignItems:"center",
    position:"absolute",
  },
  circleCoords:{
      width: 140, 
      padding:12, 
      marginLeft: 12, 
      backgroundColor:"white", 
      color: colors.textGrey,
      fontSize:16,
      fontWeight: "bold",
      textAlign:"center"
    },
  bottomCoords:{
    flexDirection:"row",
    position:"absolute",
    bottom:32,
    alignSelf:"center"
  },
  play:{
      position:"absolute",
      bottom:32,
      alignSelf:"center"
  },
  text:{
      fontWeight: "bold",
      color: colors.textGrey
  }
});