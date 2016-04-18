insert into CoreSysGroup (select UUID(),'005','温德姆酒店集团',5,0);
insert into CoreSysBrand(select UUID(),'速8','005001',(select GroupId from CoreSysGroup where groupCode='005'),'1','SuBa.png',0);