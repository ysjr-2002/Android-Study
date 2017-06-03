using Android.Media;
using Android.Runtime;

namespace FaceVisualExt
{
    class Router : MediaRouter.SimpleCallback
    {
        public override void OnRouteAdded(MediaRouter router, MediaRouter.RouteInfo info)
        {
            base.OnRouteAdded(router, info);
        }

        public override void OnRouteSelected(MediaRouter router, [GeneratedEnum] MediaRouteType type, MediaRouter.RouteInfo info)
        {
            base.OnRouteSelected(router, type, info);

        }
    }
}