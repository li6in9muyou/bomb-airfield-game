function Server() {
  this.socket = new WebSocket("ws://127.0.0.1:9898");
  this.name = "scsc";
  this.socket.onopen = function (evt) {
    console.log("Connection open");
  };

  this.socket.onmessage = function (evt) {
    console.log("Received Message: " + evt.data);
    var onMsgEvent = new CustomEvent("onMsg", { detail: { Msg: evt.data } });
    window.dispatchEvent(onMsgEvent);
  };

  this.socket.onclose = function (evt) {
    console.log("Connection closed");
  };
}
function sleep(time) {
  var t1 = Date.now();
  while (1) {
    var t2 = Date.now();
    if (t2 - t1 > time) break;
    setTimeout(()=>{},0);
  }
}
/*async function sleep(time){
  await new Promise(resolve => setTimeout(resolve, time))
}*/
function Cache() {
  /*this.stateMsg = null;
  this.isStateMsgNew = false;*/
  this.bombResult = null;
  this.isBombResultNew = false;

  /*this.setStateMsg = function (msg) {
    this.stateMsg = msg;
    this.isStateMsgNew = true;
    console.log("setStateMsg:" + msg);
  };*/
  this.setBombResult = function (br) {
    this.bombResult = br;
    this.isBombResultNew = true;
    console.log("BombResultSetted:" + br);
  };
  /*this.waitStateMsg = function () {
    while (!this.isStateMsgNew) {
      console.log("StateMsg:");
      sleep(3000);
    }
    this.isStateMsgNew = false;
    return this.stateMsg;
  };*/
  this.waitBombResult = function () {
    console.log("WaitingBombResult:");
    while (!this.isBombResultNew) {
      console.log("WaitingBombResult:");
      sleep(3000);
    }
    this.isBombResultNew = false;
    return this.bombResult;
  };
}
