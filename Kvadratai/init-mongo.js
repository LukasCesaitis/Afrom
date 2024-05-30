db.createUser(
    {
        user: "admin",
        pwd: "Test123!",
        roles: [
            {
                role: "readWrite",
                db: "MongoDB"
            }
        ]
    }
);
db.createCollection("PointsCollection");

db.PointsCollection.insert({
    "_id": "90700C24-1459-41AD-A16C-1A2756C7ADB0",
    "X": "2",
    "Y": "-2"
});