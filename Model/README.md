# Model
A baseline C# data model alongside an Entity Framework 6.x implementation for persisting the model to a SQL database. Additionally the model project implements `IdentityUser` and `IdentityDbContext` so that it can be integrated with ASP.NET Identity. The basic data model includes `User`, `Post`, and `Comment` classes, along with relationships such as `Following`, `Likes`, etc. 

## `User`
- `Id`
- `Username`
- `Comments` (1:n)
- `Posts` (1:n)
- `TaggedInPosts` (1:n)
- `Following` \[`Followers`\] (n:n)
- `LikedComments` (n:n)
- `LikedPosts` (n:n)
- `DateCreated`

## `Post`
- `Id`
- `Title`
- `Body`
- `Keywords`
- `User` [`UserId`]
- `Comments` (1:n)
- `Likes` (n:n)
- `TaggedUsers` (n:n)

## `Comment`
- `Id`
- `Body`
- `User` [`UserId`]
- `Post` [`PostId`]
- `Likes` (n:n)

## Collections
The following classes based on `KeyedCollection<T>` support all 1:n and n:n relationships used in the above classes. This supports JSON serialization while also providing an extensibility point for collection-specific members (e.g., custom indexes, filtered views, etc).
- `CommentCollection`
- `PostCollection`
- `UserCollection`

## Entity Sets
The following entity sets are defined via the `WorkbenchContext` class, which provides the `DbContext` for Entity Framework.
- `Posts`
- `Comments`


