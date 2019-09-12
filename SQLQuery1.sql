/*select * from Customers
select * from Products
select * from Categories
select * from Locations
select * from Deliverers
select * from Login
select * from Orders
select * from OrderItems
select * from OrderItemProducts
select * from Customers
select * from Sellers
select * from Payments

insert into Login values ('Seller', 'wathsala@gmail.com', '1234')
insert into Categories values('electronic', null)
insert into Categories values('sport', null)
insert into Categories values('food', null)
insert into Categories values('electrical', null)
insert into Products values('Nkia-8', '2018-8core 16 rear cam', 'original',80000,null,124,4.1,121,5,null,4,1)
insert into Products values('Huawei-p30', '2018-8core 16 rear cam', 'original',30000,null,233,3.1,80,5,null,4,1)
insert into Products values('Huawei-p30', '2018-8core 16 rear cam', 'original',30000,null,233,3.1,80,5,null,4,1)
insert into Products values('hadset', 'stereo-2.0', 'original',2000,null,200,4.9,244,10,null,4,1)
insert into Orders values (GetDate(), 1, 4, 'to be confirmed',6.7999, 79.9)
insert into OrderItems values ( 8, 4 )
insert into OrderItems values ( 5, 3)
insert into OrderItemProducts values (GetDate(),4,6)
insert into Sellers values('prabashi', 'navoda', null,077222222,null,1,'shop-Katubadda', null,null,null,6.799012,79.888700,null)
insert into Payments values (1100000, GetDate(),5)
update Login set Role='Seller' where Id=1
update Orders set Status='to be confirmed' where Id=4   'to be delivered'
//delete from Products where Id=3
/update Deliverers set DeliveryStatus='online' where Id=1
//update Categories set CategoryName='electronic' where Id=1
//insert into Deliverers values ('wathsala', 'dantha',null,'12345678',null,2,null,'car2223','online','123456v')
insert into Locations values( 6.79,79.88,1, null)
insert into Customers values ('ku', 'wathsala',null,'12345678',null,3)
update Login set Role='Customer' where Id=3*/
update Orders set CustomerLatitude=6.7991, CustomerLongitude=79.8889 where Id=3
update Orders set Status='to be confirmed' where Id=3
update Orders set Status='to be confirmed' where Id=4

update OrderItemProducts set OrderItemId=5 where Id=5
delete Orders where Id=18
delete Orders where Id=19
delete Orders where Id=20
delete Orders where Id=21
delete Orders where Id=22
insert into Payments values (1000, GetDate(), 4)
