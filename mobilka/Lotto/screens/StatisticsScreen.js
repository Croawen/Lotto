import React from 'react';
import {View, StyleSheet, Image} from 'react-native'
import BlueButton from '../components/BlueButton';
import colors from '../helper/colors';

export default class StatisticsScreen extends React.Component {
    



  render() {
    return (
      <View style ={styles.container}>
           <Image style={{width:"100%", height:"100%"}}source={require("../assets/staty.png")}/> 
            
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor:colors.white
  },
  login:{

  }
});