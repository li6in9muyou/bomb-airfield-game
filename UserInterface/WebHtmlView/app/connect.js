function Connector(){
   this.socket=null,
   this.cache=null,
   this.connect=function (des){
      var server=new Server();
      this.socket=server.socket;

      this.cache=new Cache();
      window.addEventListener("onMsg", e => {
         var Msg=JSON.parse(e.detail.Msg);
         var header=Msg.header;
         var body=Msg.body;
         if(header=="stateMsg"){
            if(body.startsWith('aiaiai')){
               const [,x,y]=body.split(",")
               const aiSuggestion = [x,y];
               if(document.getElementById("[data-ai-toggle]").checked){
                  window.doAttack(aiSuggestion)
               } else {
                  window.aiSuggestions.push(aiSuggestion)
               }
            } else {
               des.innerHTML=body;
            }
         }else if(header=="bombResult"){
            this.cache.setBombResult(body);
         }
      });
   },
   this.getYourAttack=function(){
      let msg=this.cache.waitBombResult();
      let brs=JSON.parse(msg);
      //[{"x":2,"y":2,"result":"Destroyed"},{"x":2,"y":2,"result":"Hit"},{"x":2,"y":2,"result":"Miss"}]
      return brs;//得到敌方的攻击位置和结果
   },
   this.showYourAttack=function (num){
      let res = this.getYourAttack();
      for(let i = 0;i<res.length;i++) {
         if(res[i]['y']>=10){
            num = res[i]['x']* 10 + res[i]['y'] - 10
            let key= wound(res[i]['result'])
            yourLi[num].innerHTML = `<img src='../img/wound.png' width='84' height='84'><p class="key">${key}</p>`
         }else {
            num = res[i]['x'] * 10 + res[i]['y']
            let key = wound(res[i]['result'])
            myLi[num].innerHTML = `<img src='../img/wound.png' width='84' height='84'><p class="key">${key}</p>`
         }
      }
   },  
   this.sendMyAttack=function(value){
      //[1,2]
      var coor={x:value[0],y:value[1]};
      var msg={header:"BombLocation",body:JSON.stringify(coor)};
      this.socket.send(JSON.stringify(msg));
      return true//发送我的攻击位置
   },
   this.sendMyPosition=function(value){
      //1234上右下左
      //[[1,2,1],[3,4,2],[5,6,3]]
      var aps=[];
      for(var i=0;i<value.length;i++){
         let ap={};
         ap.X=value[i][0].toString();
         ap.Y=value[i][1].toString();
         let d=value[i][2];
         if(d==1){
            ap.Direction="u";
         }else if(d==2){
            ap.Direction="r";
         }else if(d==3){
            ap.Direction="d";
         }else if(d==4){
            ap.Direction="l";
         }
         aps.push(ap);
      }
      var apRoot={aps:aps};
      var msg={header:"AirplanesPlacement",body:JSON.stringify(apRoot)};
      this.socket.send(JSON.stringify(msg));
      return true//发送飞机布局位置
   },
   this.sendMyIP=function (ip){
       //创建房间ip为空字符串 加入房间为ip地址
      var msg={header:"IpAddress",body:ip};
      this.socket.send(JSON.stringify(msg));
   }    
   /*this.myResult=function (des){
    //des是提示框元素对象 提示信息直接给它的innerText赋值就行
   }*/
}
export default Connector