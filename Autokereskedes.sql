create database Auto_kereskedes;
use Auto_kereskedes;
create table Orszag(
	OID int not null identity,
	ONev varchar(30),
	primary key (OID)
)
create table Gyartok(
	GYID int not null identity,
	Cegnev varchar(30) not null,
	primary key (GYID)
)
create table Autok(
	AID int not null identity,
	ANev varchar(30) not null,
	Ferohely int not null,
	Szeriaszam int not null,
	Keszlet int,
	GYID int not null,
	Ar int not null,
	primary key (AID),
	foreign key (GYID) references Gyartok(GYID)
)
create table Nyelvek(
	NyID int not null identity,
	NyNev varchar(30) not null,
	primary key (NyID)
)
create table Kliensek(
	KID int not null identity,
	KNev varchar(30) not null,
	Lakcim varchar(100),
	Tel int,
	primary key (KID)	
)
create table Alkalmazottak(
	AlkID int not null identity,
	AlkNev varchar(30) not null,
	Lakcim varchar(100),
	Tel int,
	Fizetes int,
	primary key (AlkID)
)
create table Reszlegek(
	RID	int not null identity,
	RNev varchar(30) not null,
	ManID int not null,
	primary key (RID),
	foreign key (ManID) references Alkalmazottak(AlkID)
)
create table NyelvTudas(
	AlkID int not null,
	NyID int not null,
	primary key(AlkID,NyID),
	foreign key(AlkID) references Alkalmazottak(AlkID),
	foreign key(NyID) references Nyelvek(NyID)
)
create table OrszagNyelv(
	NyID int not null,
	OID int not null,
	primary key (NyID,OID),
	foreign key(OID) references Orszag(OID),
	foreign key(NyID) references Nyelvek(NyID)
)
create table GyartoSzarmazas(
	GyID int not null,
	OID int not null,
	primary key (GyID,OID),
	foreign key(OID) references Orszag(OID),
	foreign key(GyID) references Gyartok(GyID)
)
create table Vasarlas(
	AID int not null,
	KID int not null,
	AlkID int not null,
	Datum date not null,
	primary key(AID,KID,Datum),
	foreign key(AID) references Autok(AID),
	foreign key(KID) references Kliensek(KID),
	foreign key(AlkID) references Alkalmazottak(AlkID)
)
create table Dolgozik(
	AlkID int not null,
	RID int not null,
	primary key(AlkID,RID),
	foreign key(AlkID) references Alkalmazottak(AlkID),
	foreign key(RID) references Reszlegek(RID)
)
create table Beszerez(
	AlkID int not null,
	GyID int not null,
	AID int not null,
	Darab int not null,
	Datum datetime not null,
	primary key (AlkID,GyID,Datum),
	foreign key(AlkID) references Alkalmazottak(AlkID),
	foreign key(GyID) references Gyartok(GyID),
	foreign key(AID) references Autok(AID)
)

ALTER TABLE Dolgozik
ADD CONSTRAINT alkid_rid_fkey FOREIGN KEY (AlkID)
REFERENCES Alkalmazottak (AlkID) ON DELETE CASCADE;

ALTER TABLE Dolgozik
ADD CONSTRAINT alkid_rid_fkey FOREIGN KEY (AlkID)
REFERENCES Alkalmazottak (AlkID) ON DELETE CASCADE;

ALTER TABLE Beszerez
ADD CONSTRAINT beszerez_gyato_id_fkey FOREIGN KEY (GyID)
REFERENCES Gyartok (GyID) ON DELETE CASCADE;

ALTER TABLE GyartoSzarmazas
ADD CONSTRAINT szarm_gyato_id_fkey FOREIGN KEY (GyID)
REFERENCES Gyartok (GyID) ON DELETE CASCADE;

ALTER TABLE Vasarlas
ADD CONSTRAINT vasarlas_auto_id_fkey FOREIGN KEY (AID)
REFERENCES Autok (AID) ON DELETE CASCADE;

ALTER TABLE NyelvTudas
ADD CONSTRAINT alka_nyelv_id_fkey FOREIGN KEY (AlkID)
REFERENCES Alkalmazottak (AlkID) ON DELETE CASCADE;

ALTER TABLE Vasarlas
ADD CONSTRAINT vasarlas_alk_id_fkey FOREIGN KEY (AlkID)
REFERENCES Alkalmazottak(AlkID) ON DELETE CASCADE;

ALTER TABLE Beszerez
ADD CONSTRAINT beszerzes_alk_id_fkey FOREIGN KEY (AlkID)
REFERENCES Alkalmazottak(AlkID) ON DELETE CASCADE;

INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Takacs Peter','Petofi utca 30.',0756435678,2200);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Dragics Lajos','Petofi utca 31.',0756235678,1800);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Orosz Almos','Petofi utca 32.',0756445678,1200);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Fuzi Imola','Petofi utca 33.',0753335678,1200);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Varga Elemer','Petofi utca 34.',0756735678,1000);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Kelemen Orsolya','Petofi utca 35.',0752335678,1800);
INSERT INTO Alkalmazottak (AlkNev,Lakcim,Tel,Fizetes) VALUES('Pal Karoly','Petofi utca 36.',0758535678,1000);

INSERT INTO Reszlegek (RNEv,ManID) VALUES('Beszerzes',2);
INSERT INTO Reszlegek (RNEv,ManID) VALUES('Eladas',6);

INSERT INTO Dolgozik (AlkID,RID) VALUES(1,1);
INSERT INTO Dolgozik (AlkID,RID) VALUES(1,2);
INSERT INTO Dolgozik (AlkID,RID) VALUES(2,1);
INSERT INTO Dolgozik (AlkID,RID) VALUES(3,2);
INSERT INTO Dolgozik (AlkID,RID) VALUES(4,2);
INSERT INTO Dolgozik (AlkID,RID) VALUES(5,1);
INSERT INTO Dolgozik (AlkID,RID) VALUES(6,2);
INSERT INTO Dolgozik (AlkID,RID) VALUES(7,1);

INSERT INTO Gyartok (Cegnev) VALUES('BMW');
INSERT INTO Gyartok (Cegnev) VALUES('Ford');
INSERT INTO Gyartok (Cegnev) VALUES('Renault');

INSERT INTO Autok (ANev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('BMW X6',6,234546723,40,1,5300);
INSERT INTO Autok (ANev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('BMW X5',5,234346723,20,1,5100);
INSERT INTO Autok (ANev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('Ford Fiesta',4,987646723,15,2,2300);
INSERT INTO Autok (ANev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('Dacia Duster',5,246546723,60,3,4900);
INSERT INTO Autok (ANev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('Ford Focus',7,894546723,33,2,3600);

INSERT INTO Nyelvek (NyNev) VALUES('Nemet');
INSERT INTO Nyelvek (NyNev) VALUES('Angol');
INSERT INTO Nyelvek (NyNev) VALUES('Orosz');
INSERT INTO Nyelvek (NyNev) VALUES('Magyar');
INSERT INTO Nyelvek (NyNev) VALUES('Roman');

INSERT INTO Orszag (ONev) VALUES('Nemetorszag');
INSERT INTO Orszag (ONev) VALUES('Anglia');
INSERT INTO Orszag (ONev) VALUES('Franciaorszag');
INSERT INTO Orszag (ONev) VALUES('Magyarorszag');

INSERT INTO OrszagNyelv (NyID,OID) VALUES(1,1);
INSERT INTO OrszagNyelv (NyID,OID) VALUES(1,2);
INSERT INTO OrszagNyelv (NyID,OID) VALUES(2,2);
INSERT INTO OrszagNyelv (NyID,OID) VALUES(3,2);
INSERT INTO OrszagNyelv (NyID,OID) VALUES(3,5);
INSERT INTO OrszagNyelv (NyID,OID) VALUES(4,4);

INSERT INTO NyelvTudas (AlkID,NyID) VALUES(1,1);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(1,2);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(3,3);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(3,4);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(3,1);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(4,1);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(6,2);
INSERT INTO NyelvTudas (AlkID,NyID) VALUES(3,5);

INSERT INTO Kliensek (KNev,Lakcim,Tel) VALUES('Pal Peter','Dorobantilor 12.',0756879645);
INSERT INTO Kliensek (KNev,Lakcim,Tel) VALUES('Peter Pal','Dorobantilor 13.',0756879565);
INSERT INTO Kliensek (KNev,Lakcim,Tel) VALUES('Lakatos Eszter','Dorobantilor 14.',0756347645);
INSERT INTO Kliensek (KNev,Lakcim,Tel) VALUES('Peter Monika','Dorobantilor 19.',0754379645);

INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(6,1,1,'2012-05-12');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(6,2,2,'2012-05-15');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(7,1,3,'2012-05-17');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(7,1,1,'2012-05-12');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(10,2,1,'2012-05-12');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(7,1,3,'2012-05-23');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(13,3,4,'2012-05-18');
INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(11,4,2,'2012-05-19');
