#include <iostream>
#include <cstring>
using namespace std;
int conLen,conValue[200],  //大陆的数量和价值，从1开始
    counLen,counOf[200],counArmy[200],counBelong[200],  //国家数量、大陆编号、军队数、属于的玩家编号，前三个从1开始
    namesLen=0;  //现在已经知道几个玩家的名字
//这里是因为原来规定的数据格式里，玩家是用名字而不是编号区分的，比较麻烦。
//在这个AI内部，玩家编号从0开始
int myCard=0,cardType[20],cardNum[20],  //我的卡片数、卡片的类型（1-4）、卡片编号（也是对应国家号，wildcard编号与国家无关）
    myNum,  //我的名字的编号，还是上面的原因
    attFrom,attTo,  //本次我打算进攻的双方
    findSucc;  //局部用的变量
char buf[1000],  //输入字符串的缓存
     conn[200][200]={0},  //连通情况
     names[10][50],  //玩家的名字，第一维从0开始
     armiesmore=0,  //同输入
     markCType[5][20],ctLen[5];  //换卡片时计算用
const char* myName="zh";
void interpret(char* str,int from){  //建边的时候比较麻烦，我只想到一下读入一行，然后自己把里面的数字拆开
     int i=0,l=strlen(buf),a=0;
     while(str[i]!=' ')i++;
     i++;
     while(i<=l){
       while(str[i]!=' '&&str[i]!=0){
         a=a*10+str[i]-'0';
         i++;
       }
       conn[from][a]=1;
       a=0;
       i++;
     }
}
int main(){
    int a,b;
    while(1){
      do{
        cin.getline(buf,1000);
      }while(strlen(buf)<3);
      if(strcmp(buf,"[continents]")==0){
        cin>>conLen;
        for(int i=1;i<=conLen;i++)
          cin>>a>>buf>>conValue[i];
      }else if(strcmp(buf,"[countries]")==0){
        cin>>counLen;
        for(int i=1;i<=counLen;i++)
          cin>>a>>buf>>counOf[i];
      }else if(strcmp(buf,"[borders]")==0){
        cin.getline(buf,1000);
        for(int i=1;i<=counLen;i++){
          cin.getline(buf,1000);
          interpret(buf,i);
        }
      }else if(strcmp(buf,"[showarmies]")==0){  //把军队数记下来。因为开始说的原因，这里要把玩家名字翻译成AI自己理解的玩家编号
        cin>>a;
        for(int i=1;i<=counLen;i++){
          cin>>a>>buf>>b;
          if(strcmp(buf,"*")==0){
            counArmy[i]=0;
            counBelong[i]=-1;
            continue;
          }
          a=-1;
          for(int j=0;j<namesLen;j++)
            if(strcmp(names[j],buf)==0){
              a=j;
              break;
            }
          if(a==-1){
            strcpy(names[namesLen],buf);
            a=namesLen;
            namesLen++;
          }
          counBelong[i]=a;
          counArmy[i]=b;
        }
      }else if(strcmp(buf,"[showcards]")==0){
        cin>>myCard;
        for(int i=0;i<myCard;i++){
          cin>>buf>>cardNum[i];
          if(buf[2]=='v')cardType[i]=1;
          else if(buf[2]=='n')cardType[i]=2;
          else if(buf[2]=='f')cardType[i]=3;
          else if(buf[2]=='l')cardType[i]=4;
        }
      }else if(strcmp(buf,"[armiesmore]")==0){
        armiesmore=1;
        for(int i=0;i<namesLen;i++)
          if(strcmp(names[i],myName)==0)myNum=i;
      }else if(strcmp(buf,"[place]")==0){
        cin>>a;
        if(armiesmore==0)
          cout<<"autoplace"<<endl;  //没放满时我在这里写的自动放置
        else{
          int i;
          for(i=1;i<=counLen;i++)
            if(counBelong[i]==myNum)break;
          cout<<"placearmies "<<i<<" 1"<<endl;  //随便找个地方放得了
        }
      }else if(strcmp(buf,"[attack]")==0){
        int i;
        for(i=1;i<=counLen;i++)
          if(counBelong[i]==myNum&&counArmy[i]>2)  //有3个兵就打
            for(int j=1;j<=counLen;j++)
              if(conn[i][j]==1&&counBelong[j]!=myNum){
                attFrom=i;
                attTo=j;
                cout<<"attack "<<i<<" "<<j<<endl;
                goto out1;
              }
        out1:
        if(i>counLen)cout<<"endattack"<<endl;
      }else if(strcmp(buf,"[roll]")==0){  //这里的意思应该是，如果你没有占领，就一直会发roll指令，除非你retreat或者死光了
        a=counArmy[attFrom]-1;
        if(a>3)a=3;
        if(a==0)cout<<"retreat"<<endl;
        else cout<<"roll "<<a<<endl;
      }else if(strcmp(buf,"[trade]")==0){  //随便换换卡片
        findSucc=0;
        if(myCard<3)cout<<"endtrade"<<endl;
        else{
          for(int i=1;i<=4;i++)ctLen[i]=0;
          for(int i=0;i<myCard;i++){
            markCType[cardType[i]][ctLen[cardType[i]]]=i;
            ctLen[cardType[i]]++;
          }
          for(int i=1;i<=3;i++){
               if(ctLen[i]>=3){
                   cout<<"trade"<<" "<<cardNum[markCType[i][0]]<<" "<<cardNum[markCType[i][1]]<<" "<<cardNum[markCType[i][2]]<<endl;
                   break;
               }
               else if(ctLen[i]+ctLen[4]>=3){
                  cout<<"trade";
                  for(int j=0;j<ctLen[i];j++)
                    cout<<" "<<cardNum[markCType[i][j]];
                  for(int j=0;j<3-ctLen[i];j++)
                    cout<<" "<<cardNum[markCType[4][j]];
                  cout<<endl;
                  findSucc=1;
                  break;
               }
            }
          a=(ctLen[1]>0)+(ctLen[2]>0)+(ctLen[3]>0);
          if(findSucc==0&&a+ctLen[4]>=3){
            cout<<"trade";
            for(int i=1;i<=3;i++)
              if(ctLen[i]>0)cout<<" "<<cardNum[markCType[i][0]];
            for(int i=0;i<3-a;i++)
              cout<<" "<<cardNum[markCType[4][i]];
            cout<<endl;
            findSucc=1;
          }
          if(findSucc==0)cout<<"endtrade"<<endl;
        }
      }else if(strcmp(buf,"[won]")==0){  //获胜后把所有可以的兵都挪过去
        cout<<"move all"<<endl;
      }else if(strcmp(buf,"[move]")==0){  //不移动
        cout<<"nomove"<<endl;
      }else if(strcmp(buf,"[end]")==0){
        return 0;
      }
    }
}
