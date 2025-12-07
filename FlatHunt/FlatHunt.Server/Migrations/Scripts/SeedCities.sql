INSERT INTO LunCities(Name, GeoId, GeoType, Url) VALUES
('Kyiv', '10009580', 'city', '/rent/kyiv/flats'),
('Lviv', '10012684', 'city', '/rent/lviv/flats'),
('Odessa', '10016589', 'city', '/rent/odesa/flats'),
('Vinnitsa', '10003908', 'city', '/rent/vinnytsia/flats'),
('Dnipro', '10006463', 'city', '/rent/dnipro/flats'),
('Zhytomyr', '10007252', 'city', '/rent/zhytomyr/flats'),
('Zaporizhzhia', '10007846', 'city', '/rent/zp/flats'),
('Ivano Frankivsk', '10008717', 'city', '/rent/if/flats'),
('Kropyvnytskyi', '10011240', 'city', '/rent/kr/flats'),
('Lutsk', '10012656', 'city', '/rent/volyn/flats'),
('Mykolaiv', '10013982', 'city', '/rent/mykolaiv/flats'),
('Poltava', '10018885', 'city', '/rent/poltava/flats'),
('Rivne', '10019894', 'city', '/rent/rivne/flats'),
('Sumy', '10022820', 'city', '/rent/sumy/flats'),
('Ternopil', '10023304', 'city', '/rent/ternopil/flats'),
('Uzhhorod', '10023968', 'city', '/rent/uz/flats'),
('Kharkiv', '10024345', 'city', '/rent/kharkiv/flats'),
('Kherson', '10024395', 'city', '/rent/kherson/flats'),
('Cherkasy', '10025145', 'city', '/rent/cherkasy/flats'),
('Chernivtsi', '10025207', 'city', '/rent/chernivtsi/flats'),
('Chernihiv', '10025209', 'city', '/rent/chernihiv/flats'),
('Khmelnytskyi', '10024474', 'city', '/rent/khmelnytskyi/flats');


INSERT INTO Cities (Name, LunCityId)
SELECT v.Name, lc.Id
FROM (VALUES
    ('Kyiv',         '10009580'),
    ('Lviv',         '10012684'),
    ('Odessa',       '10016589'),
    ('Vinnitsa',     '10003908'),
    ('Dnipro',       '10006463'),
    ('Zhytomyr',     '10007252'),
    ('Zaporizhzhia', '10007846'),
    ('Ivano Frankivsk', '10008717'),
    ('Kropyvnytskyi','10011240'),
    ('Lutsk',        '10012656'),
    ('Mykolaiv',     '10013982'),
    ('Poltava',      '10018885'),
    ('Rivne',        '10019894'),
    ('Sumy',         '10022820'),
    ('Ternopil',     '10023304'),
    ('Uzhhorod',     '10023968'),
    ('Kharkiv',      '10024345'),
    ('Kherson',      '10024395'),
    ('Cherkasy',     '10025145'),
    ('Chernivtsi',   '10025207'),
    ('Chernihiv',    '10025209'),
    ('Khmelnytskyi', '10024474')
) AS v(Name, GeoId)
JOIN LunCities lc ON lc.GeoId = v.GeoId;